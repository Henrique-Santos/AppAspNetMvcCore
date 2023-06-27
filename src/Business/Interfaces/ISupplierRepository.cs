using Business.Models;

namespace Business.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        // Métodos especializados para o Fornecedor
        Task<Supplier> GetSupplierAddressAsync(Guid id);
        Task<Supplier> GetProductsAndSupplierAddressAsync(Guid id);
    }
}