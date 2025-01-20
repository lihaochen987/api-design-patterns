namespace backend.Product.InfrastructureLayer;

public interface IProductRepository
{
    Task<DomainModels.Product?> GetProductAsync(long id);
    Task CreateProductAsync(DomainModels.Product product);
    Task DeleteProductAsync(long id);
    Task UpdateProductAsync(DomainModels.Product product);
}
