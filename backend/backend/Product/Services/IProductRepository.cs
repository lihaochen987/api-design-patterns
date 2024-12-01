namespace backend.Product.Services;

public interface IProductRepository
{
    Task<DomainModels.Product?> GetProductAsync(long id);
    Task CreateProductAsync(DomainModels.Product product);
    Task DeleteProductAsync(DomainModels.Product product);
    Task UpdateProductAsync(DomainModels.Product product);
}
