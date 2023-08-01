using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using App.ViewModels;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using App.Extensions;

namespace App.Controllers
{
    [Authorize]
    public class SuppliersController : BaseController
    {
        private readonly ISupplierService _supplierService;
        private readonly ISupplierRepository _supplierRepository;        
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierRepository supplierRepository, ISupplierService supplierService, IMapper mapper, INotifier notifier) : base(notifier)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {         
            return View(_mapper.Map<List<SupplierViewModel>>(await _supplierRepository.GetAllAsync()));
        }

        [AllowAnonymous]
        [Route("dados-do-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddressAsync(id));
            if (supplier is null) return NotFound();
            return View(supplier);
        }

        [ClaimsAuthorize("Supplier", "Add")]
        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Supplier", "Add")]
        [Route("novo-fornecedor")]
        [HttpPost]        
        public async Task<IActionResult> Create(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return View(supplierViewModel);            
            var supplier = _mapper.Map<Supplier>(supplierViewModel);            
            await _supplierService.AddAsync(supplier);
            if (!IsValidOperation()) return View(supplierViewModel);
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [Route("editar-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetProductsAndSupplierAddressAsync(id));
            if (supplier is null) return NotFound();                      
            return View(supplier);
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost]        
        public async Task<IActionResult> Edit(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id) return NotFound();            
            if (!ModelState.IsValid) return View(supplierViewModel);
            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierService.UpdateAsync(supplier);
            if (!IsValidOperation()) return View(_mapper.Map<SupplierViewModel>(await _supplierRepository.GetProductsAndSupplierAddressAsync(id)));
            return RedirectToAction(nameof(Index));                        
        }

        [ClaimsAuthorize("Supplier", "Delete")]
        [Route("excluir-fornecedor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddressAsync(id));
            if (supplier is null)  return NotFound();            
            return View(supplier);
        }

        [ClaimsAuthorize("Supplier", "Delete")]
        [Route("excluir-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]        
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddressAsync(id));
            if (supplier is null) return NotFound();
            await _supplierService.DeleteAsync(id);
            if (!IsValidOperation()) return View(supplier);
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> UpdatedAddress(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddressAsync(id));
            if (supplier is null) return NotFound();
            return PartialView("_UpdatedAddress", new SupplierViewModel { Address = supplier.Address });
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpPost]        
        public async Task<IActionResult> UpdatedAddress(SupplierViewModel supplierViewModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Document");
            if (!ModelState.IsValid) return PartialView("_UpdatedAddress", supplierViewModel);
            await _supplierService.UpdateAddressAsync(_mapper.Map<Address>(supplierViewModel.Address));
            if (!IsValidOperation()) PartialView("_UpdatedAddress", supplierViewModel);
            var url = Url.Action(nameof(GetAddress), "Suppliers", new { id = supplierViewModel.Address.SupplierId });
            return Json(new { success = true, url });
        }

        [AllowAnonymous]
        [Route("obter-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddressAsync(id));
            if (supplier is null) return NotFound();
            return PartialView("_AddressDetail", supplier);
        }
    }
}