using backend.Database;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        // Fetch all products from the SQLite database
        var products = await context.Products.ToListAsync();
        return Ok(products);
    }
}