using backend.Product.Database;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.Services;

public class ProductRepository(ProductDbContext context) : IProductRepository
{
    public async Task<DomainModels.BaseProduct?> GetProductAsync(long id)
    {
        return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateProductAsync(DomainModels.BaseProduct baseProduct)
    {
        context.Products.Add(baseProduct);
        await context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(DomainModels.BaseProduct baseProduct)
    {
        context.Products.Remove(baseProduct);
        await context.SaveChangesAsync();
    }

    public async Task ReplaceProductAsync(DomainModels.BaseProduct baseProduct)
    {
        baseProduct.Replace(baseProduct.Name, baseProduct.Pricing, baseProduct.Category, baseProduct.Dimensions);
        await context.SaveChangesAsync();
    }
}