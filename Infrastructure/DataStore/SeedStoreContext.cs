using System.Text.Json;
using Core.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataStore
{
    public class SeedStoreContext
    {
        public static async Task SeedAsync(RepositoryContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/DataStore/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    // foreach (var brand in brandsData)
                    // {
                    //     context.ProductBrands.Add(brand);
                    // }
                }
            }
            catch (Exception ex)
            {
            
          }
        }
    }
}