using AutoMapper;
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
        var product = Product.MapCreateRequestToProduct(request); 
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

    [Route("/product")]
    [HttpPatch]
    public async Task<ActionResult<Product>> UpdateProduct([FromQuery] int id, [FromBody] UpdateProductRequest request)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }
        
        await context.SaveChangesAsync();
        return Ok(product);
    }

    // [Route("/product")]
    // [HttpPut]
    // public async Task<ActionResult<Product>> ReplaceProduct([FromQuery] int id, [FromBody] ReplaceProductRequest request)
    // {
    //     if (request.Resource == null)
    //     {
    //         return BadRequest("Invalid product data.");
    //     }
    //
    //     var existingProduct = await context.Products.FindAsync(request.Resource.ProductId);
    //
    //     if (existingProduct != null)
    //     {
    //         existingProduct.ProductName = request.Resource.ProductName;
    //         existingProduct.ProductPrice = request.Resource.ProductPrice;
    //         existingProduct.ProductCategory = request.Resource.ProductCategory;
    //         await context.SaveChangesAsync();
    //         return Ok(existingProduct);
    //     }
    //
    //     context.Products.Add(request.Resource);
    //     await context.SaveChangesAsync();
    //     return CreatedAtAction(nameof(GetProduct), new { productId = request.Resource.ProductId }, request.Resource);
    // }

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