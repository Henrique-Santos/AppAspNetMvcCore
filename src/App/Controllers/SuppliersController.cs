using Microsoft.AspNetCore.Mvc;
using App.ViewModels;
using Business.Interfaces;
using Business.Models;
using AutoMapper;

namespace App.Controllers
{
    public class SuppliersController : BaseController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index()
        {         
            return View(_mapper.Map<List<SupplierViewModel>>(await _supplierRepository.GetAllAsync()));
        }
        
        public async Task<IActionResult> Details(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddressAsync(id));
            if (supplier is null) return NotFound();
            return View(supplier);
        }

        public IActionResult Create()
        {
            return View();
        }
                        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return View(supplierViewModel);            
            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierRepository.AddAsync(supplier);  
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetProductsAndSupplierAddressAsync(id));
            if (supplier is null) return NotFound();                      
            return View(supplier);
        }
                        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id) return NotFound();            
            if (!ModelState.IsValid) return View(supplierViewModel);
            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierRepository.UpdateAsync(supplier);
            return RedirectToAction(nameof(Index));                        
        }
        
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddressAsync(id));
            if (supplier is null)  return NotFound();            
            return View(supplier);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddressAsync(id));
            if (supplier is null) return NotFound();
            await _supplierRepository.DeleteAsync(id);                        
            return RedirectToAction(nameof(Index));
        }
    }
}