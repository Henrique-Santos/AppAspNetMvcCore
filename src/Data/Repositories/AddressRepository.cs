using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(MyDbContext db) : base(db) { }

        public async Task<Address> GetAddressBySupplier(Guid supplierId)
        {
            return await Db.Addresses.AsNoTracking().FirstOrDefaultAsync(x => x.SupplierId == supplierId);
        }
    }
}