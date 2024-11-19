using backend.Product.Database;
using backend.Product.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.Services;

public class ProductPricingRepository(ProductDbContext context) : IProductPricingRepository
{
    public async Task<ProductPricingView?> GetProductPricingAsync(long id)
    {
        var product = await context.Set<ProductPricingView>()
            .FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }
}