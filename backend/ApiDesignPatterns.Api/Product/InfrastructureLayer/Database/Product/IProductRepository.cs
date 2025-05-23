using backend.Product.DomainModels;

namespace backend.Product.InfrastructureLayer.Database.Product;

public interface IProductRepository
{
    Task<DomainModels.Product?> GetProductAsync(long id);
    Task<List<DomainModels.Product>> GetProductsByIds(List<long> productIds);
    Task<PetFood?> GetPetFoodProductAsync(long id);
    Task<GroomingAndHygiene?> GetGroomingAndHygieneProductAsync(long id);
    Task<long> CreateProductAsync(DomainModels.Product product);
    Task CreatePetFoodProductAsync(PetFood product);
    Task CreateGroomingAndHygieneProductAsync(GroomingAndHygiene product);
    Task DeleteProductAsync(long id);
    Task DeleteProductsAsync(IEnumerable<long> ids);
    Task<long> UpdateProductAsync(DomainModels.Product product);
    Task UpdatePetFoodProductAsync(PetFood product);
    Task UpdateGroomingAndHygieneProductAsync(GroomingAndHygiene product);
    Task<IEnumerable<long>> CreateProductsAsync(IEnumerable<DomainModels.Product> products);
    Task CreatePetFoodProductsAsync(IEnumerable<PetFood> petFoodProducts);
    Task CreateGroomingAndHygieneProductsAsync(IEnumerable<GroomingAndHygiene> groomingProducts);
}
