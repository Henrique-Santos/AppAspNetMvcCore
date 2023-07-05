using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        // Por IRepository ser uma class abstrata (abstract) é necessario passar o argumento do seu construtor (base)
        public ProductRepository(MyDbContext db) : base(db) { }

        public async Task<IEnumerable<Product>> GetProductsBySupplierAsync(Guid supplierId)
        {
            return await SearchAsync(x => x.SupplierId == supplierId);
        }

        public async Task<Product> GetSupplierAndProductAsync(Guid id)
        {
            return await Db.Products.AsNoTracking().Include(x => x.Supplier).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Product>> GetSuppliersAndProductsAsync()
        {
            return await Db.Products.AsNoTracking().Include(x => x.Supplier).OrderBy(x => x.Name).ToListAsync();
        }
    }
}