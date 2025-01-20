using System.Data;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Queries;
using backend.Review.InfrastructureLayer.Queries;
using Dapper;

namespace backend.Product.InfrastructureLayer;

public class ProductPricingRepository(IDbConnection dbConnection) : IProductPricingRepository
{
    public async Task<ProductPricingView?> GetProductPricingAsync(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<ProductPricingView>(ProductPricingQueries.GetProductPricing,
            new { Id = id });
    }
}
