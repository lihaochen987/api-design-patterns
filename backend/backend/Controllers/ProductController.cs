using backend.Contracts;
using backend.Database;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductRequest request)
    {
        if (request.Resource == null)
        {
            return BadRequest();
        }

        context.Products.Add(request.Resource);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = request.Resource.ProductId }, request.Resource);
    }

    [HttpGet]
    public async Task<ActionResult<Product>> GetProduct([FromQuery] GetProductRequest request)
    {
        var product = await context.Products.FindAsync(request.Id);
        return Ok(product);
    }
}