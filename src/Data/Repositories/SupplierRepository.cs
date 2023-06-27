using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(MyDbContext db) : base(db) { }

        public async Task<Supplier> GetProductsAndSupplierAddressAsync(Guid id)
        {
            return await Db.Suppliers.AsNoTracking().Include(x => x.Address).Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Supplier> GetSupplierAddressAsync(Guid id)
        {
            return await Db.Suppliers.AsNoTracking().Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}