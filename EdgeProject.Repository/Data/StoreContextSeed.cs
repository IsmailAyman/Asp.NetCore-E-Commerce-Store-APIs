using EdgeProject.Core.Entities;
using EdgeProject.Core.Entities.Order_Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EdgeProject.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../EdgeProject.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        await dbContext.ProductBrands.AddAsync(brand);

                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            if (!dbContext.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../EdgeProject.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                if (types?.Count > 0)
                {
                    foreach (var type in types)
                    {
                        await dbContext.ProductTypes.AddAsync(type);

                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            if (!dbContext.Products.Any())
            {
                var productsData = File.ReadAllText("../EdgeProject.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        await dbContext.Products.AddAsync(product);

                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            if (!dbContext.DeliveryMethods.Any())
            {
                var methodsData = File.ReadAllText("../EdgeProject.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(methodsData);

                if (deliveryMethods?.Count > 0)
                {
                    foreach (var method in deliveryMethods)
                    {
                        await dbContext.DeliveryMethods.AddAsync(method);

                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }

    }
}
