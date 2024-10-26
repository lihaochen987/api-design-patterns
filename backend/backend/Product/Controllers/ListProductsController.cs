using backend.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace backend.Product.Controllers;

[ApiController]
public class ListProductsController(ApplicationDbContext context) : ControllerBase
{
    [Route("products")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(request.PageToken) && int.TryParse(request.PageToken, out var lastSeenProductId))
        {
            query = query.Where(p => p.Id > lastSeenProductId);
        }

        var products = await query
            .OrderBy(p => p.Id)
            .Take(request.MaxPageSize + 1)
            .ToListAsync();

        string? nextPageToken = null;
        if (products.Count > request.MaxPageSize)
        {
            var lastProductInPage = products[request.MaxPageSize - 1];
            nextPageToken = lastProductInPage.Id.ToString();
            products.RemoveAt(request.MaxPageSize);
        }

        var response = new ListProductsResponse
        {
            Results = products,
            NextPageToken = nextPageToken
        };

        return Ok(response);
    }
}