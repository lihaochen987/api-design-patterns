using System.Linq.Expressions;
using backend.Product.Contracts;
using backend.Product.Database;
using backend.Product.ViewModels;
using backend.Shared;
using backend.Shared.CelSpecParser;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.Services;

public class ProductRepository(ProductDbContext context) : IProductRepository
{
    public async Task<ProductView?> GetProductViewByIdAsync(long id)
    {
        return await context.Set<ProductView>()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ProductListResult<ProductView>> ListProductsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var query = context.Set<ProductView>().AsQueryable();

        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out var lastSeenProductId))
        {
            query = query.Where(p => p.Id > lastSeenProductId);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            var filterExpression = BuildFilterExpression(filter);
            query = query.Where(filterExpression);
        }

        var products = await query
            .OrderBy(p => p.Id)
            .Take(maxPageSize + 1)
            .ToListAsync();

        var paginatedProducts = PaginateProducts(products, maxPageSize, out var nextPageToken);

        return new ProductListResult<ProductView>
        {
            Items = paginatedProducts,
            NextPageToken = nextPageToken
        };
    }

    private static Expression<Func<ProductView, bool>> BuildFilterExpression(string filter)
    {
        var parser = new CelParser<ProductView>(new TypeParser());
        var tokens = parser.Tokenize(filter);
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

        var lastProductInPage = existingProducts[maxPageSize - 1];
        nextPageToken = lastProductInPage.Id.ToString();
        return existingProducts.Take(maxPageSize).ToList();
    }
}