namespace backend.Product.ApplicationLayer;

public interface IProductApplicationService
{
    Task<DomainModels.Product?> GetProductAsync(long id);
    Task<long> CreateProductAsync(DomainModels.Product product);
    Task DeleteProductAsync(DomainModels.Product product);
}
