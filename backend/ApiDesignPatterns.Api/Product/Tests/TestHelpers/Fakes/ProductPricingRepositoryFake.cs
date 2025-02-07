using System.Collections.ObjectModel;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
using backend.Product.InfrastructureLayer.Database.ProductPricing;

namespace backend.Product.Tests.TestHelpers.Fakes;

public class ProductPricingRepositoryFake : Collection<ProductPricingView>, IProductPricingRepository
{
    private Dictionary<string, int> CallCount { get; } = new();

    public Task<ProductPricingView?> GetProductPricingAsync(long id)
    {
        ProductPricingView? productPricingView = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(productPricingView);
    }
}
