using backend.Product.DomainModels.Views;

namespace backend.Product.InfrastructureLayer.Database.ProductPricing;

public interface IProductPricingRepository
{
    Task<ProductPricingView?> GetProductPricingAsync(long id);
}
