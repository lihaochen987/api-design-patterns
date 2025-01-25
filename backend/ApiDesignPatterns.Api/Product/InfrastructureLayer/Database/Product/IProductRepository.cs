using backend.Product.DomainModels;

namespace backend.Product.InfrastructureLayer.Database.Product;

public interface IProductRepository
{
    Task<DomainModels.Product?> GetProductAsync(long id);
    Task<PetFood?> GetPetFoodProductAsync(long id);
    Task<GroomingAndHygiene?> GetGroomingAndHygieneProductAsync(long id);
    Task<long> CreateProductAsync(DomainModels.Product product);
    Task CreatePetFoodProductAsync(PetFood product);
    Task CreateGroomingAndHygieneProductAsync(GroomingAndHygiene product);
    Task DeleteProductAsync(long id);
    Task<long> UpdateProductAsync(DomainModels.Product product);
    Task UpdatePetFoodProductAsync(PetFood product);
    Task UpdateGroomingAndHygieneProductAsync(GroomingAndHygiene product);
}
