using System.Collections.ObjectModel;
using backend.Product.DomainModels;
using backend.Product.InfrastructureLayer.Database.Product;

namespace backend.Product.Tests.TestHelpers.Fakes;

public class ProductRepositoryFake : Collection<DomainModels.Product>, IProductRepository
{
    public bool IsDirty { get; set; }

    public Task<DomainModels.Product?> GetProductAsync(long id)
    {
        DomainModels.Product? product = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<PetFood?> GetPetFoodProductAsync(long id)
    {
        PetFood? product = (PetFood?)this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<GroomingAndHygiene?> GetGroomingAndHygieneProductAsync(long id)
    {
        GroomingAndHygiene? product = (GroomingAndHygiene?)this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task CreatePetFoodProductAsync(PetFood product)
    {
        IsDirty = true;
        Add(product);
        return Task.FromResult(product.Id);
    }

    public Task CreateGroomingAndHygieneProductAsync(GroomingAndHygiene product)
    {
        IsDirty = true;
        Add(product);
        return Task.FromResult(product.Id);
    }

    public Task<long> CreateProductAsync(DomainModels.Product product)
    {
        IsDirty = true;
        Add(product);
        return Task.FromResult(product.Id);
    }

    public Task DeleteProductAsync(long id)
    {
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
        int index = IndexOf(this.FirstOrDefault(p => p.Id == product.Id) ??
                            throw new InvalidOperationException());
        this[index] = product;
        IsDirty = true;

        return Task.FromResult(product.Id);
    }

    public Task UpdatePetFoodProductAsync(PetFood product)
    {
        int index = IndexOf(this.FirstOrDefault(p => p.Id == product.Id) ??
                            throw new InvalidOperationException());
        this[index] = product;
        IsDirty = true;

        return Task.FromResult(product.Id);
    }

    public Task UpdateGroomingAndHygieneProductAsync(GroomingAndHygiene product)
    {
        int index = IndexOf(this.FirstOrDefault(p => p.Id == product.Id) ??
                            throw new InvalidOperationException());
        this[index] = product;
        IsDirty = true;

        return Task.FromResult(product.Id);
    }
}
