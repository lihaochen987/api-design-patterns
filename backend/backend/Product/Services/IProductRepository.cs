namespace backend.Product.Services;

public interface IProductRepository
{
    Task<DomainModels.BaseProduct?> GetProductAsync(long id);
    Task CreateProductAsync(DomainModels.BaseProduct baseProduct);
    Task DeleteProductAsync(DomainModels.BaseProduct baseProduct);
    Task ReplaceProductAsync(DomainModels.BaseProduct baseProduct);
}