using backend.Product.DomainModels;

namespace backend.Product.InfrastructureLayer;

public interface IProductRepository
{
    Task<DomainModels.Product?> GetProductAsync(long id);
    Task<long> CreateProductAsync(DomainModels.Product product);
    Task CreatePetFoodProductAsync(PetFood product);
    Task CreateGroomingAndHygieneProductAsync(GroomingAndHygiene product);
    Task DeleteProductAsync(long id);
    Task UpdateProductAsync(DomainModels.Product product);
}
