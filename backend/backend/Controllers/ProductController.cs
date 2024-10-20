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

    [Route("/product")]
    [HttpGet]
    public async Task<ActionResult<Product>> GetProduct([FromQuery] GetProductRequest request)
    {
        var product = await context.Products.FindAsync(request.Id);
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

    [Route("/product")]
    [HttpPatch]
    public async Task<ActionResult<Product>> UpdateProduct([FromQuery] UpdateProductRequest request)
    {
        if (request.Resource == null)
        {
            return BadRequest("Invalid product details.");
        }

        var product = await context.Products.FindAsync(request.Resource.ProductId);
        await context.SaveChangesAsync();
        return Ok(product);
    }

    [Route("/product")]
    [HttpPut]
    public async Task<ActionResult<Product>> ReplaceProduct([FromQuery] ReplaceProductRequest request)
    {
        if (request.Resource == null)
        {
            return BadRequest("Invalid product data.");
        }

        var existingProduct = await context.Products.FindAsync(request.Resource.ProductId);

        if (existingProduct != null)
        {
            existingProduct.ProductName = request.Resource.ProductName;
            existingProduct.ProductPrice = request.Resource.ProductPrice;
            existingProduct.ProductCategory = request.Resource.ProductCategory;
            await context.SaveChangesAsync();
            return Ok(existingProduct);
        }

        context.Products.Add(request.Resource);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { productId = request.Resource.ProductId }, request.Resource);
    }

    [Route("/product")]
    [HttpDelete]
    public async Task<ActionResult> DeleteProduct([FromQuery] DeleteProductRequest request)
    {
        var product = await context.Products.FindAsync(request.Id);
        if (product == null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return NoContent();
    }
}