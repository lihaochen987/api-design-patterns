using backend.Contracts;
using backend.Database;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
public class ProductController(ApplicationDbContext context) : ControllerBase
{
    [Route("/product")]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductRequest request)
    {
        if (!Product.TryParse(request, out var product, out var errorMessages) || product == null)
            return BadRequest(errorMessages);
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
    }

    [Route("/product")]
    [HttpGet]
    public async Task<ActionResult<Product>> GetProduct([FromQuery] GetProductRequest request)
    {
        if (!long.TryParse(request.Id, out var id)) return BadRequest();
        var product = await context.Products.FindAsync(id);
        return Ok(product);
    }

    [Route("/products")]
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

    // Todo: Actually implement partial Updates because it's hard
    [Route("/product")]
    [HttpPatch]
    public async Task<ActionResult<Product>> UpdateProduct([FromQuery] long id, [FromBody] UpdateProductRequest request)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        await context.SaveChangesAsync();
        return Ok(product);
    }

    [Route("/product")]
    [HttpPut]
    public async Task<ActionResult<Product>> ReplaceProduct([FromQuery] long id, [FromBody] ReplaceProductRequest request)
    {
        var existingProduct = await context.Products.FindAsync(id);
        if (existingProduct == null) return NotFound();

        existingProduct.UpdateProductDetails(request, out var errorMessages);

        if (errorMessages.Any())
        {
            return BadRequest(new { Errors = errorMessages });
        }

        await context.SaveChangesAsync();
    
        return Ok(existingProduct);
    }

    [Route("/product")]
    [HttpDelete]
    public async Task<ActionResult> DeleteProduct([FromQuery] DeleteProductRequest request)
    {
        if (!long.TryParse(request.Id, out var id)) return BadRequest();
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return NoContent();
    }
}