using System.Collections.ObjectModel;
using backend.Product.InfrastructureLayer;

namespace backend.Product.Tests.TestHelpers.Fakes;

public class ProductRepositoryFake : Collection<DomainModels.Product>, IProductRepository
{
    public bool IsDirty { get; set; }
    public Dictionary<string, int> CallCount { get; } = new();

    private void IncrementCallCount(string methodName)
    {
        if (!CallCount.TryAdd(methodName, 1))
        {
            CallCount[methodName]++;
        }
    }

    public Task<DomainModels.Product?> GetProductAsync(long id)
    {
        IncrementCallCount(nameof(GetProductAsync));
        DomainModels.Product? product = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task CreateProductAsync(DomainModels.Product product)
    {
        IncrementCallCount(nameof(CreateProductAsync));
        Add(product);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task DeleteProductAsync(DomainModels.Product product)
    {
        IncrementCallCount(nameof(DeleteProductAsync));
        Remove(product);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task UpdateProductAsync(DomainModels.Product product)
    {
        IncrementCallCount(nameof(UpdateProductAsync));
        int index = IndexOf(this.FirstOrDefault(p => p.Id == product.Id) ??
                            throw new InvalidOperationException());
        this[index] = product;
        IsDirty = true;

        return Task.CompletedTask;
    }
}
