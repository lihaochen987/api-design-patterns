using backend.Product.DomainModels.Views;

namespace backend.Product.InfrastructureLayer;

public interface IProductPricingRepository
{
    Task<ProductPricingView?> GetProductPricingAsync(long id);
}
