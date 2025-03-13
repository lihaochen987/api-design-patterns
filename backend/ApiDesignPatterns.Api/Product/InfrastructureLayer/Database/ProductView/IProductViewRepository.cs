using backend.Product.ApplicationLayer.Queries.ListProducts;

namespace backend.Product.InfrastructureLayer.Database.ProductView;

public interface IProductViewRepository
{
    Task<DomainModels.Views.ProductView?> GetProductView(long id);
    Task<PagedProducts> ListProductsAsync(string? pageToken, string? filter, int maxPageSize);
}
