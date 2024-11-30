using System.Linq.Expressions;
using backend.Product.Contracts;
using backend.Product.Database;
using backend.Product.ViewModels;
using backend.Shared;
using backend.Shared.CelSpecParser;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.Services;

public class ProductViewRepository(ProductDbContext context) : IProductViewRepository
{
    public async Task<ProductView?> GetProductView(long id) =>
        await context.Set<ProductView>()
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<ProductListResult<ProductView>> ListProductsAsync(
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

        List<ProductView> paginatedProducts = PaginateProducts(products, maxPageSize, out string? nextPageToken);

        return new ProductListResult<ProductView> { Items = paginatedProducts, NextPageToken = nextPageToken };
    }

    private static Expression<Func<ProductView, bool>> BuildFilterExpression(string filter)
    {
        CelParser<ProductView> parser = new(new TypeParser());
        List<CelToken> tokens = parser.Tokenize(filter);
        return parser.ParseFilter(tokens);
    }

    private static List<ProductView> PaginateProducts(
        List<ProductView> existingProducts,
        int maxPageSize,
        out string? nextPageToken)
    {
        if (existingProducts.Count <= maxPageSize)
        {
            nextPageToken = null;
            return existingProducts;
        }

        ProductView lastProductInPage = existingProducts[maxPageSize - 1];
        nextPageToken = lastProductInPage.Id.ToString();
        return existingProducts.Take(maxPageSize).ToList();
    }
}
