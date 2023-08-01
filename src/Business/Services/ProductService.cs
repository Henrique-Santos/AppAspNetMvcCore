using Business.Interfaces;
using Business.Models;
using Business.Models.Validations;

namespace Business.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository, INotifier notifier) : base(notifier)
        {
            _productRepository = productRepository;
        }

        public async Task AddAsync(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) return;
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) return;
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }
    }
}