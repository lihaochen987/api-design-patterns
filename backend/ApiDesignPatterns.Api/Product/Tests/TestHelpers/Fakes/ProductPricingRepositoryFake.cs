using System.Collections.ObjectModel;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;

namespace backend.Product.Tests.TestHelpers.Fakes;

public class ProductPricingRepositoryFake : Collection<ProductPricingView>, IProductPricingRepository
{
    public Dictionary<string, int> CallCount { get; } = new();
    public bool IsDirty { get; set; }

    private void IncrementCallCount(string methodName)
    {
        if (!CallCount.TryAdd(methodName, 1))
        {
            CallCount[methodName]++;
        }
    }

    public Task<ProductPricingView?> GetProductPricingAsync(long id)
    {
        IncrementCallCount(nameof(GetProductPricingAsync));
        ProductPricingView? productPricingView = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(productPricingView);
    }
}
