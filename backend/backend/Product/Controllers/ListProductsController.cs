using System.Linq.Expressions;
using backend.Database;
using backend.Shared.CelSpecParser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers;

[Route("products")]
[ApiController]
public class ListProductsController(
    ApplicationDbContext context,
    GetProductExtensions extensions)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List products", Tags = ["Products"])]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(request.PageToken) && int.TryParse(request.PageToken, out var lastSeenProductId))
        {
            query = query.Where(p => p.Id > lastSeenProductId);
        }

        if (!string.IsNullOrEmpty(request.Filter))
        {
            var filterExpression = BuildFilterExpression(request.Filter);
            query = query.Where(filterExpression);
        }

        var products = await query
            .OrderBy(p => p.Id)
            .Take(request.MaxPageSize + 1)
            .ToListAsync();

        var paginatedProducts = PaginateProducts(
            products,
            request.MaxPageSize,
            out var nextPageToken);

        var productResponses = paginatedProducts.Select(extensions.ToGetProductResponse).ToList();

        var response = new ListProductsResponse
        {
            Results = productResponses,
            NextPageToken = nextPageToken
        };

        return Ok(response);
    }

    private static Expression<Func<DomainModels.Product, bool>> BuildFilterExpression(string filter)
    {
        var parser = new CelParser<DomainModels.Product>();
        var tokens = parser.Tokenize(filter);
        return parser.ParseFilter(tokens);
    }

    private static List<DomainModels.Product> PaginateProducts(
        List<DomainModels.Product> existingProducts,
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