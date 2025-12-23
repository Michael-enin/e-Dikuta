using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly RepositoryContext _repositoryContext;
        public ProductRepository(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public void AddProduct(Product product)
        {
            _repositoryContext.Products.Add(product);
        }

        public void deleteProduct(Product product)
        {
            _repositoryContext.Products.Remove(product);
        }

        public async Task<Product?> GetProdctByIdAsync(int id)
        {
            return await _repositoryContext.Products
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductBrand)
            .FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<IReadOnlyList<Product?>> GetProductsAsync()
        {
            return await _repositoryContext.Products
             .Include(p => p.ProductBrand)
             .Include(p => p.ProductType)
             .ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return _repositoryContext.Products.Any(x => x.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _repositoryContext.SaveChangesAsync() > 0;
        }

        public void updateProduct(Product product)
        {
            _repositoryContext.Entry(product).State = EntityState.Modified;
        }

    }
}