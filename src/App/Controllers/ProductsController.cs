using Microsoft.AspNetCore.Mvc;
using App.ViewModels;
using Business.Interfaces;
using AutoMapper;
using Business.Models;
using App.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, ISupplierRepository supplierRepository, IMapper mapper, IProductService productService, INotifier notifier) : base(notifier)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _productService = productService;
        }

        [AllowAnonymous]
        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {            
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetSuppliersAndProductsAsync()));
        }

        [AllowAnonymous]
        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProduct(id);
            if (productViewModel == null) return NotFound();            
            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Add")]
        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            var product = await SetSuppliers(new ProductViewModel());
            return View(product);
        }

        [ClaimsAuthorize("Product", "Add")]
        [Route("novo-produto")]
        [HttpPost]        
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await SetSuppliers(productViewModel);
            if (!ModelState.IsValid) return View(productViewModel);
            var imagePrefix = Guid.NewGuid() + "_";
            if (!await UploadFileAsync(productViewModel.ImageUpload, imagePrefix)) return View(productViewModel);
            productViewModel.Image = imagePrefix + productViewModel.ImageUpload.FileName;
            await _productService.AddAsync(_mapper.Map<Product>(productViewModel));
            if (!IsValidOperation()) return View(productViewModel);
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Product", "Edit")]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await GetProduct(id);            
            if (product == null) return NotFound();                        
            return View(product);
        }

        [ClaimsAuthorize("Product", "Edit")]
        [Route("editar-produto/{id:guid}")]
        [HttpPost]        
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();   
            var productUpdated = await GetProduct(id);
            productViewModel.Suppliers = productUpdated.Suppliers;
            productViewModel.Image = productUpdated.Image;
            if (!ModelState.IsValid) return View(productViewModel);
            if(productViewModel.ImageUpload is not null)
            {
                var imagePrefix = Guid.NewGuid() + "_";
                if (!await UploadFileAsync(productViewModel.ImageUpload, imagePrefix)) return View(productViewModel);
                productUpdated.Image = imagePrefix + productViewModel.ImageUpload.FileName;
            }
            productUpdated.Name = productViewModel.Name;
            productUpdated.Description = productViewModel.Description;
            productUpdated.Value = productViewModel.Value;
            await _productService.UpdateAsync(_mapper.Map<Product>(productUpdated));
            if (!IsValidOperation()) return View(productViewModel);
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Product", "Delete")]
        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var product = await GetProduct(id);
            if (product == null) return NotFound();            
            return View(product);
        }

        [ClaimsAuthorize("Product", "Delete")]
        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]        
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await GetProduct(id);
            if (product == null) return NotFound();
            await _productService.DeleteAsync(id);
            if (!IsValidOperation()) return View(product);
            TempData["Success"] = "Produto excluido com sucesso!";
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