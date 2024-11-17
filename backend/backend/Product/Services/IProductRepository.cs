using backend.Product.Contracts;
using backend.Product.ViewModels;

namespace backend.Product.Services;

public interface IProductRepository
{
    Task<ProductView?> GetProductViewByIdAsync(long id);
    Task<DomainModels.Product?> GetProductByIdAsync(long id);
    Task<ProductListResult<ProductView>> ListProductsAsync(string? pageToken, string? filter, int maxPageSize);
    Task CreateProductAsync(DomainModels.Product product);
    Task DeleteProductAsync(DomainModels.Product product);
    Task ReplaceProductAsync(DomainModels.Product product);
}