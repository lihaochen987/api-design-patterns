using backend.Product.ProductControllers;

namespace backend.Product.ApplicationLayer;

public interface IProductApplicationService
{
    Task<GetProductResponse?> GetProductAsync(long id);
    Task<long> CreateProductAsync(DomainModels.Product product);
    Task DeleteProductAsync(long id);
}
