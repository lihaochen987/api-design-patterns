using backend.Product.ViewModels;

namespace backend.Product.Services;

public interface IProductPricingRepository
{
    Task<ProductPricingView?> GetProductPricingAsync(long id);
}
