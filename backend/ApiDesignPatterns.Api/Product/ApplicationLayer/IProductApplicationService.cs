using backend.Product.ProductControllers;
using backend.Product.Services;

namespace backend.Product.ApplicationLayer;

public interface IProductApplicationService
{
    Task<DomainModels.Product?> GetProductAsync(long id);
    Task CreateProductAsync(DomainModels.Product product);
    Task DeleteProductAsync(DomainModels.Product product);

    Task ReplaceProductAsync(DomainModels.Product product);

    Task UpdateProductAsync(
        UpdateProductRequest request,
        DomainModels.Product product);
}
