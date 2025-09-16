using System.Collections.Generic;
using Core.Entities;
namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProdctByIdAsync(int id);
        Task<IReadOnlyList<Product?>> GetProductsAsync();
    }
}