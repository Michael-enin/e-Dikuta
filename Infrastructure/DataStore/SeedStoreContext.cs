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
            var logger = loggerFactory.CreateLogger<SeedStoreContext>();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            try
            {
                 var Rootpath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "Infrastructure", "DataStore", "SeedData");
                if (!context.ProductBrands.Any())
                {
                   var path = Path.Combine(Rootpath, "brands.json");
                    var brandsData = await File.ReadAllTextAsync(path);
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData, options);
                    if (brands?.Any() == true)
                    {

                        context.ProductBrands.AddRange(brands);
                        await context.SaveChangesAsync();
                        logger.LogInformation("ProductBrands Seeded");
                    }
                }
                if (!context.Products.Any())
                {
                    var path = Path.Combine(Rootpath, "products.json");
                    var data = await File.ReadAllTextAsync(path);
                    var products = JsonSerializer.Deserialize<List<Product>>(data);
                    if (products != null)
                    {
                        Console.WriteLine("products file parsed");
                        foreach (var product in products)
                        {
                            context.Products.Add(product);
                        }
                        await context.SaveChangesAsync();
                    }
                }
                if (!context.ProductTypes.Any())
                {
                    var path = Path.Combine(Rootpath, "productsTypes.json");
                    var data = await File.ReadAllTextAsync(path);
                    var products = JsonSerializer.Deserialize<List<ProductType>>(data);
                    if (products != null)
                    {
                        foreach (var product in products)
                        {
                            context.ProductTypes.Add(product);
                        }
                        await context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                // throw new Exception("Data is not seeded", ex);
                logger.LogError(ex.Message);
            }
        }
    }
}