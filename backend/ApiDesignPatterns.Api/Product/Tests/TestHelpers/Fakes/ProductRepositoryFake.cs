using System.Collections.ObjectModel;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.Tests.TestHelpers.Builders;

namespace backend.Product.Tests.TestHelpers.Fakes;

public class ProductRepositoryFake :
    Collection<DomainModels.Product>, IProductRepository, IGetProduct, ICreateProduct, IDeleteProduct, IUpdateProduct
{
    public bool IsDirty { get; set; }

    public void AddProduct()
    {
        var product = new ProductTestDataBuilder().Build();
        Add(product);
    }

    public void AddProduct(long id)
    {
        var product = new ProductTestDataBuilder().WithId(id).Build();
        Add(product);
    }

    public void AddProduct(long id, Category category)
    {
        var product = new ProductTestDataBuilder().WithId(id).WithCategory(category).Build();
        Add(product);
    }

    public Task<DomainModels.Product?> GetProductAsync(long id)
    {
        DomainModels.Product? product = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<List<DomainModels.Product>> GetProductsByIds(List<long> productIds)
    {
        var products = this.Where(x => productIds.Contains(x.Id)).ToList();
        return Task.FromResult(products);
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
        return Task.CompletedTask;
    }

    public Task CreateGroomingAndHygieneProductAsync(GroomingAndHygiene product)
    {
        IsDirty = true;
        Add(product);
        return Task.CompletedTask;
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

    public Task DeleteProductsAsync(IEnumerable<long> ids)
    {
        foreach (long id in ids)
        {
            var product = this.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                continue;
            }

            Remove(product);
        }

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

    public Task<IEnumerable<long>> CreateProductsAsync(IEnumerable<DomainModels.Product> products)
    {
        IsDirty = true;
        IEnumerable<DomainModels.Product> productList = products.ToList();
        foreach (var product in productList)
        {
            Add(product);
        }

        return Task.FromResult(productList.Select(x => x.Id));
    }

    public Task CreatePetFoodProductsAsync(IEnumerable<PetFood> petFoodProducts)
    {
        IsDirty = true;
        IEnumerable<DomainModels.Product> productList = petFoodProducts.ToList();
        foreach (var product in productList)
        {
            Add(product);
        }

        return Task.CompletedTask;
    }

    public Task CreateGroomingAndHygieneProductsAsync(IEnumerable<GroomingAndHygiene> groomingProducts)
    {
        IsDirty = true;
        IEnumerable<DomainModels.Product> productList = groomingProducts.ToList();
        foreach (var product in productList)
        {
            Add(product);
        }

        return Task.CompletedTask;
    }
}
