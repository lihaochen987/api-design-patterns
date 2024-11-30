using System.Collections.ObjectModel;
using backend.Product.Services;

namespace backend.Product.Tests.Fakes;

public class ProductRepositoryFake : Collection<DomainModels.Product>, IProductRepository
{
    public bool IsDirty { get; set; }

    public Task<DomainModels.Product?> GetProductAsync(long id)
    {
        DomainModels.Product? product = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task CreateProductAsync(DomainModels.Product product)
    {
        Add(product);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task DeleteProductAsync(DomainModels.Product product)
    {
        Remove(product);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task ReplaceProductAsync(DomainModels.Product product)
    {
        int index = IndexOf(this.FirstOrDefault(p => p.Id == product.Id) ??
                            throw new InvalidOperationException());
        this[index] = product;
        IsDirty = true;

        return Task.CompletedTask;
    }
}
