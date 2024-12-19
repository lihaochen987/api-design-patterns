using backend.Product.DomainModels.Views;

namespace backend.Product.InfrastructureLayer;

public interface IProductViewRepository
{
    Task<ProductView?> GetProductView(long id);
    Task<(List<ProductView>, string?)> ListProductsAsync(string? pageToken, string? filter, int maxPageSize);
}
