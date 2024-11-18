using System.Collections.ObjectModel;
using backend.Product.Contracts;
using backend.Product.Services;
using backend.Product.ViewModels;

namespace backend.Product.Tests.Fakes;

public class ProductViewRepositoryFake : Collection<ProductView>, IProductViewRepository
{
    public bool IsDirty { get; set; }

    public Task<ProductView?> GetProductView(long id)
    {
        var productView = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(productView);
    }

    public Task<ProductListResult<ProductView>> ListProductsAsync(string? pageToken, string? filter, int maxPageSize)
    {
        throw new NotImplementedException();
    }
}