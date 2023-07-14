using Microsoft.AspNetCore.Mvc;
using App.ViewModels;
using Business.Interfaces;
using AutoMapper;
using Business.Models;

namespace App.Controllers
{
    public class ProductsController : BaseController
    {

        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, ISupplierRepository supplierRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {            
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetSuppliersAndProductsAsync()));
        }
        
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProduct(id);
            if (productViewModel == null) return NotFound();            
            return View(productViewModel);
        }
        
        public async Task<IActionResult> Create()
        {
            var product = await SetSuppliers(new ProductViewModel());
            return View(product);
        }
                        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await SetSuppliers(productViewModel);
            if (!ModelState.IsValid) return View(productViewModel);
            var imagePrefix = Guid.NewGuid() + "_";
            if (!await UploadFileAsync(productViewModel.ImageUpload, imagePrefix)) return View(productViewModel);
            productViewModel.Image = imagePrefix + productViewModel.ImageUpload.FileName;
            await _productRepository.AddAsync(_mapper.Map<Product>(productViewModel));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await GetProduct(id);            
            if (product == null) return NotFound();                        
            return View(product);
        }
                        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();            
            if (!ModelState.IsValid) return View(productViewModel);
            await _productRepository.UpdateAsync(_mapper.Map<Product>(productViewModel));
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid id)
        {

            var product = GetProduct(id);
            if (product == null) return NotFound();            
            return View(product);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = GetProduct(id);
            if (product == null) return NotFound();
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<ProductViewModel> GetProduct(Guid id)
        {
            var product = _mapper.Map<ProductViewModel>(await _productRepository.GetSupplierAndProductAsync(id));
            product.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAllAsync());
            return product;
        }

        private async Task<ProductViewModel> SetSuppliers(ProductViewModel product)
        {            
            product.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAllAsync());
            return product;
        }

        private async Task<bool> UploadFileAsync(IFormFile file, string imagePrefix)
        {
            if (file.Length <= 0) return false;
            // Monta o caminho para a pasta wwwroot/images
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imagePrefix + file.FileName);
            // Verificando se já existe um arquivo com esse nome
            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "There is already a file with that name");
                return false;
            }
            // Gravando o arquivo de imagem no disco
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return true;
        }
    }
}