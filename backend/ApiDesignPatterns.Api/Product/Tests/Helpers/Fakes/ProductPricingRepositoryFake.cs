using System.Collections.ObjectModel;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;

namespace backend.Product.Tests.Helpers.Fakes;

public class ProductPricingRepositoryFake : Collection<ProductPricingView>, IProductPricingRepository
{
    public Task<ProductPricingView?> GetProductPricingAsync(long id)
    {
        ProductPricingView? productPricingView = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(productPricingView);
    }
}
