using System.Data;
using backend.Product.DomainModels.Views;
using Dapper;

namespace backend.Product.InfrastructureLayer.Database.ProductPricing;

public class ProductPricingRepository(IDbConnection dbConnection) : IProductPricingRepository
{
    public async Task<ProductPricingView?> GetProductPricingAsync(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<ProductPricingView>(ProductPricingQueries.GetProductPricing,
            new { Id = id });
    }
}
