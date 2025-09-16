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

        public async Task<Product?> GetProdctByIdAsync(int id)
        {
            return await _repositoryContext.Products.FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<IReadOnlyList<Product?>> GetProductsAsync()
        {
            return await _repositoryContext.Products.ToListAsync();
        }
    }
}