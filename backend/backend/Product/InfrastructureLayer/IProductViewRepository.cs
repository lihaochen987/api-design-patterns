using backend.Product.Contracts;
using backend.Product.DomainModels.Views;

namespace backend.Product.InfrastructureLayer;

public interface IProductViewRepository
{
    Task<ProductView?> GetProductView(long id);
    Task<ProductListResult<ProductView>> ListProductsAsync(string? pageToken, string? filter, int maxPageSize);
}
