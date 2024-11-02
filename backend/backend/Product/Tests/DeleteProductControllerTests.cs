using AutoFixture;
using backend.Database;
using backend.Product.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

[Collection("SequentialExecutionCollection")]
public class DeleteProductControllerTests : IDisposable
{
    private readonly Fixture _fixture = new();
    private readonly DeleteProductController _controller;
    private readonly ApplicationDbContext _dbContext;

    public DeleteProductControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=localhost;Database=mytestdatabase;Username=myusername;Password=mypassword")
            .Options;

        var db = new ApplicationDbContext(options);
        db.Database.EnsureCreated();
        _dbContext = db;
        _controller = new DeleteProductController(_dbContext);
    }

    [Fact]
    public async Task DeleteProduct_Should_ReturnNoContent_When_ProductExists()
    {
        var product = new ProductTestDataBuilder().Build();
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var result = await _controller.DeleteProduct(product.Id, new DeleteProductRequest());

        result.ShouldBeOfType<NoContentResult>();
        var deletedProduct = await _dbContext.Products.FindAsync(product.Id);
        deletedProduct.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteProduct_Should_ReturnNotFound_When_ProductDoesNotExist()
    {
        var result = await _controller.DeleteProduct(999, new DeleteProductRequest());

        result.ShouldBeOfType<NotFoundResult>();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}