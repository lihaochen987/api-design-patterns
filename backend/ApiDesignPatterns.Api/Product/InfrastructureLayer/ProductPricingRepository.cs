using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.InfrastructureLayer;

public class ProductPricingRepository(ProductDbContext context) : IProductPricingRepository
{
    public async Task<ProductPricingView?> GetProductPricingAsync(long id)
    {
        ProductPricingView? product = await context.Set<ProductPricingView>()
            .FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }
}
