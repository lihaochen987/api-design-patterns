using backend.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.Controllers;

[ApiController]
public class ListProductsController(ApplicationDbContext context) : ControllerBase
{
    [Route("products")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        var products = await context.Products.ToListAsync();

        var response = new ListProductsResponse
        {
            Results = products
        };

        return Ok(response);
    }
}