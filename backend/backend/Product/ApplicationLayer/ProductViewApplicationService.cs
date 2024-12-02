using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
using backend.Product.ProductControllers;

namespace backend.Product.ApplicationLayer;

public class ProductViewApplicationService(
    IProductViewRepository repository)
    : IProductViewApplicationService
{
    public async Task<ProductView?> GetProductView(long id, GetProductRequest request)
    {
        // Prepare
        ProductView? product = await repository.GetProductView(id);

        // Apply
        return product;
    }

    public async Task<(List<ProductView>, string?)> ListProductsAsync(ListProductsRequest request)
    {
        // Prepare
        (List<ProductView> products, string? nextPageToken) = await repository.ListProductsAsync(
            request.PageToken,
            request.Filter,
            request.MaxPageSize);

        // Apply
        return (products, nextPageToken);
    }
}
