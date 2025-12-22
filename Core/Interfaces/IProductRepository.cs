using System.Collections.Generic;
using Core.Entities;
namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProdctByIdAsync(int id);
        Task<IReadOnlyList<Product?>> GetProductsAsync();
        void updateProduct(Product product);
        void deleteProduct(Product product);
        void AddProduct(Product product);
        bool ProductExists(int id);
        Task<bool> SaveChangesAsync();
    }
}