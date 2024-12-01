using backend.Product.ProductControllers;

namespace backend.Product.ApplicationLayer;

public interface IProductViewApplicationService
{
    Task<string?> GetProductView(long id, GetProductRequest request);
    Task<ListProductsResponse> ListProductsAsync(ListProductsRequest request);
}
