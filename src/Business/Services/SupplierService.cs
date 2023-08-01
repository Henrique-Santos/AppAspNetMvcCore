using Business.Interfaces;
using Business.Models;
using Business.Models.Validations;

namespace Business.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressRepository _addressRepository;

        public SupplierService(ISupplierRepository supplierRepository, IAddressRepository addressRepository, INotifier notifier) : base(notifier)
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
        }

        public async Task AddAsync(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidation(), supplier) || !ExecuteValidation(new AddressValidation(), supplier.Address))
            {
                return;
            }
            if (_supplierRepository.SearchAsync(x => x.Document == supplier.Document).Result.Any()) 
            {
                Notify("Já existe um fornecedor com esse mesmo documento informado");
                return;
            }
            await _supplierRepository.AddAsync(supplier);            
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidation(), supplier))
            {
                return;
            }
            if (_supplierRepository.SearchAsync(x => x.Document == supplier.Document && x.Id != supplier.Id).Result.Any())
            {
                Notify("Já existe um fornecedor com esse mesmo documento informado");
                return;
            }
            await _supplierRepository.UpdateAsync(supplier);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (_supplierRepository.GetProductsAndSupplierAddressAsync(id).Result.Products.Any())
            {
                Notify("O fornecedor possui produtos cadastrados");
                return;
            }
            await _supplierRepository.DeleteAsync(id);                        
        }

        public async Task UpdateAddressAsync(Address address)
        {
            if (!ExecuteValidation(new AddressValidation(), address))
            {
                return;
            }
            await _addressRepository.UpdateAsync(address);
        }

        public void Dispose()
        {
            _addressRepository?.Dispose();
            _supplierRepository?.Dispose();
        }
    }
}