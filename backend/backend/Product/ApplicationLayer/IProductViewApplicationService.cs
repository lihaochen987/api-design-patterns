using backend.Product.DomainModels.Views;
using backend.Product.ProductControllers;

namespace backend.Product.ApplicationLayer;

public interface IProductViewApplicationService
{
    Task<ProductView?> GetProductView(long id, GetProductRequest request);
    Task<(List<ProductView>, string?)> ListProductsAsync(ListProductsRequest request);
}
