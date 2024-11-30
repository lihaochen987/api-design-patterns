using System.Collections.ObjectModel;
using backend.Product.Services;
using backend.Product.ViewModels;

namespace backend.Product.Tests.Fakes;

public class ProductPricingRepositoryFake : Collection<ProductPricingView>, IProductPricingRepository
{
    public Task<ProductPricingView?> GetProductPricingAsync(long id)
    {
        ProductPricingView? productPricingView = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(productPricingView);
    }
}
