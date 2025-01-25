namespace backend.Product.InfrastructureLayer.Database.ProductView;

public interface IProductViewRepository
{
    Task<DomainModels.Views.ProductView?> GetProductView(long id);
    Task<(List<DomainModels.Views.ProductView>, string?)> ListProductsAsync(string? pageToken, string? filter, int maxPageSize);
}
