using System.Linq.Expressions;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database;
using backend.Shared;
using backend.Shared.CelSpec;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.InfrastructureLayer;

public class ProductViewRepository(
    ProductDbContext context)
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
            Expression<Func<ProductView, bool>> filterExpression = BuildFilterExpression(filter);
            query = query.Where(filterExpression);
        }

        List<ProductView> products = await query
            .OrderBy(p => p.Id)
            .Take(maxPageSize + 1)
            .ToListAsync();

        List<ProductView> paginatedProducts = Paginate(products, maxPageSize, out string? nextPageToken);

        return (paginatedProducts, nextPageToken);
    }

    private static Expression<Func<ProductView, bool>> BuildFilterExpression(string filter)
    {
        CelParser<ProductView> parser = new(new TypeParser());
        List<CelToken> tokens = parser.Tokenize(filter);
        return parser.ParseFilter(tokens);
    }

    private static List<ProductView> Paginate(
        List<ProductView> existingItems,
        int maxPageSize,
        out string? nextPageToken)
    {
        if (existingItems.Count <= maxPageSize)
        {
            nextPageToken = null;
            return existingItems;
        }

        ProductView lastItemInPage = existingItems[maxPageSize - 1];
        nextPageToken = lastItemInPage.Id.ToString();
        return existingItems.Take(maxPageSize).ToList();
    }
}
