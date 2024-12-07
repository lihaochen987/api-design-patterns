using System.Collections.ObjectModel;
using backend.Product.Contracts;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;

namespace backend.Product.Tests.Helpers.Fakes;

public class ProductViewRepositoryFake : Collection<ProductView>, IProductViewRepository
{
    public bool IsDirty { get; set; }

    public Task<ProductView?> GetProductView(long id)
    {
        ProductView? productView = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(productView);
    }

    public Task<(List<ProductView>, string?)> ListProductsAsync(string? pageToken, string? filter, int maxPageSize) =>
        throw new NotImplementedException();
}
