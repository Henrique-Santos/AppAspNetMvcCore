using Business.Models;

namespace Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsBySupplierAsync(Guid supplierId);
        Task<IEnumerable<Product>> GetSuppliersAndProductsAsync();
        Task<Product> GetSupplierAndProductAsync(Guid id);

    }
}