using System.Linq.Expressions;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database;
using backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.InfrastructureLayer;

public class ProductViewRepository(
    ProductDbContext context,
    QueryService<ProductView> queryService)
    : IProductViewRepository
{
    public async Task<ProductView?> GetProductView(long id) =>
        await context.Set<ProductView>()
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<(List<ProductView>, string?)> ListProductsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        IQueryable<ProductView> query = context.Set<ProductView>().AsQueryable();

        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenProductId))
        {
            query = query.Where(p => p.Id > lastSeenProductId);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            Expression<Func<ProductView, bool>> filterExpression = queryService.BuildFilterExpression(filter);
            query = query.Where(filterExpression);
        }

        List<ProductView> products = await query
            .OrderBy(p => p.Id)
            .Take(maxPageSize + 1)
            .ToListAsync();

        List<ProductView> paginatedProducts = queryService.Paginate(products, maxPageSize, out string? nextPageToken);

        return (paginatedProducts, nextPageToken);
    }
}
