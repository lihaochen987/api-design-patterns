using backend.Product.Database;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.Services;

public class ProductRepository(ProductDbContext context) : IProductRepository
{
    public async Task<DomainModels.Product?> GetProductAsync(long id)
    {
        return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateProductAsync(DomainModels.Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(DomainModels.Product product)
    {
        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }

    public async Task ReplaceProductAsync(DomainModels.Product product)
    {
        product.Replace(product.Name, product.Pricing, product.Category, product.Dimensions);
        await context.SaveChangesAsync();
    }
}