using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
using backend.Product.ProductControllers;

namespace backend.Product.ApplicationLayer;

public class ProductViewQueryApplicationService(
    IProductViewRepository repository)
    : IProductViewQueryApplicationService
{
    public async Task<ProductView?> GetProductView(long id)
    {
        ProductView? product = await repository.GetProductView(id);
        return product;
    }

    public async Task<(List<ProductView>, string?)> ListProductsAsync(ListProductsRequest request)
    {
        (List<ProductView> products, string? nextPageToken) = await repository.ListProductsAsync(
            request.PageToken,
            request.Filter,
            request.MaxPageSize);
        return (products, nextPageToken);
    }
}
