using backend.Product.Contracts;
using backend.Product.ViewModels;

namespace backend.Product.Services;

public interface IProductViewRepository
{
    Task<ProductView?> GetProductView(long id);
    Task<ProductListResult<ProductView>> ListProductsAsync(string? pageToken, string? filter, int maxPageSize);
}   