using System.Collections.ObjectModel;
using backend.Product.Services;

namespace backend.Product.Tests.Fakes;

public class ProductRepositoryFake : Collection<DomainModels.BaseProduct>, IProductRepository
{
    public bool IsDirty { get; set; }

    public Task<DomainModels.BaseProduct?> GetProductAsync(long id)
    {
        var product = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task CreateProductAsync(DomainModels.BaseProduct baseProduct)
    {
        Add(baseProduct);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task DeleteProductAsync(DomainModels.BaseProduct baseProduct)
    {
        Remove(baseProduct);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task ReplaceProductAsync(DomainModels.BaseProduct baseProduct)
    {
        var index = IndexOf(item: this.FirstOrDefault(p => p.Id == baseProduct.Id) ??
                                  throw new InvalidOperationException());
        this[index] = baseProduct;
        IsDirty = true;

        return Task.CompletedTask;
    }
}