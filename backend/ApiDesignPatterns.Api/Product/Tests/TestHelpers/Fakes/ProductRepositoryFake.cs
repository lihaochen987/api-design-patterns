using System.Collections.ObjectModel;
using backend.Product.DomainModels;
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

    public Task<PetFood?> GetPetFoodProductAsync(long id)
    {
        IncrementCallCount(nameof(GetPetFoodProductAsync));
        PetFood? product = this.FirstOrDefault(p => p.Id == id) as PetFood;
        return Task.FromResult(product);
    }

    public Task<GroomingAndHygiene?> GetGroomingAndHygieneProductAsync(long id)
    {
        IncrementCallCount(nameof(GetGroomingAndHygieneProductAsync));
        GroomingAndHygiene? product = this.FirstOrDefault(p => p.Id == id) as GroomingAndHygiene;
        return Task.FromResult(product);
    }

    public Task CreatePetFoodProductAsync(PetFood product)
    {
        IncrementCallCount(nameof(CreatePetFoodProductAsync));
        IsDirty = true;
        Add(product);
        return Task.FromResult(product.Id);
    }

    public Task CreateGroomingAndHygieneProductAsync(GroomingAndHygiene product)
    {
        IncrementCallCount(nameof(CreateGroomingAndHygieneProductAsync));
        IsDirty = true;
        Add(product);
        return Task.FromResult(product.Id);
    }

    public Task<long> CreateProductAsync(DomainModels.Product product)
    {
        IncrementCallCount(nameof(CreateProductAsync));
        IsDirty = true;
        Add(product);
        return Task.FromResult(product.Id);
    }

    public Task DeleteProductAsync(long id)
    {
        IncrementCallCount(nameof(DeleteProductAsync));
        var product = this.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return Task.CompletedTask;
        }

        Remove(product);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task<long> UpdateProductAsync(DomainModels.Product product)
    {
        IncrementCallCount(nameof(UpdateProductAsync));
        int index = IndexOf(this.FirstOrDefault(p => p.Id == product.Id) ??
                            throw new InvalidOperationException());
        this[index] = product;
        IsDirty = true;

        return Task.FromResult(product.Id);
    }

    public Task UpdatePetFoodProductAsync(PetFood product)
    {
        IncrementCallCount(nameof(UpdatePetFoodProductAsync));
        int index = IndexOf(this.FirstOrDefault(p => p.Id == product.Id) ??
                            throw new InvalidOperationException());
        this[index] = product;
        IsDirty = true;

        return Task.FromResult(product.Id);
    }

    public Task UpdateGroomingAndHygieneProductAsync(GroomingAndHygiene product)
    {
        IncrementCallCount(nameof(UpdateGroomingAndHygieneProductAsync));
        int index = IndexOf(this.FirstOrDefault(p => p.Id == product.Id) ??
                            throw new InvalidOperationException());
        this[index] = product;
        IsDirty = true;

        return Task.FromResult(product.Id);
    }
}
