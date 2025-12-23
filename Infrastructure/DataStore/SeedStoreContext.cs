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
                if (!context.ProductTypes.Any())
                {
                    var path = Path.Combine(Rootpath, "productTypes.json");
                    var data = await File.ReadAllTextAsync(path);
                    var productTypes = JsonSerializer.Deserialize<List<ProductType>>(data);
                    if (productTypes != null)
                    {
                        Console.WriteLine("products file parsed");
                        foreach (var productType in productTypes)
                        {
                            context.ProductTypes.Add(productType);
                        }
                        await context.SaveChangesAsync();
                    }
                }

                if (!context.Products.Any())
                {
                    var path = Path.Combine(Rootpath, "products.json");
                    var data = await File.ReadAllTextAsync(path);
                    var products = JsonSerializer.Deserialize<List<Product>>(data);
                    if (products != null)
                    {
                        foreach (var product in products)
                        {
                            // brannd and type by name
                            var brand = context.ProductBrands.FirstOrDefault(x => x.Name == product.ProductBrand.Name);
                            var type = context.ProductTypes.FirstOrDefault(x => x.Name == product.ProductType.Name);
                            if (brand == null || type == null)
                            {
                                throw new Exception($"Product `{product.Name}` has invalid Brand or Type");
                            }
                            product.ProductBrandId = brand.Id;
                            product.ProductTypeId = type.Id;

                            product.ProductBrand = null;
                            product.ProductType = null;
                            context.Products.Add(product);
                        }
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // throw new Exception("Data is not seeded", ex);
                // but this swallows the error
                logger.LogError(ex.Message);
                //must throw the error
                throw;
            }
        }
    }
}