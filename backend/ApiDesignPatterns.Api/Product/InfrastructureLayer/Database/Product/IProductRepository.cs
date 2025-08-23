using backend.Product.DomainModels;

namespace backend.Product.InfrastructureLayer.Database.Product;

public interface IProductRepository
{
    Task<List<DomainModels.Product>> GetProductsByIds(List<long> productIds);
    Task DeleteProductsAsync(IEnumerable<long> ids);
    Task<IEnumerable<long>> CreateProductsAsync(IEnumerable<DomainModels.Product> products);
    Task CreatePetFoodProductsAsync(IEnumerable<PetFood> petFoodProducts);
    Task CreateGroomingAndHygieneProductsAsync(IEnumerable<GroomingAndHygiene> groomingProducts);
}
