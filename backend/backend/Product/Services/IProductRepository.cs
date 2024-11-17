using backend.Product.Contracts;
using backend.Product.ViewModels;

namespace backend.Product.Services;

public interface IProductRepository
{
    Task<ProductView?> GetProductViewByIdAsync(long id);
    Task<ProductListResult<ProductView>> ListProductsAsync(string? pageToken, string? filter, int maxPageSize);
}