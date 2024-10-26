using AutoFixture;
using backend.Database;
using backend.Product.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

public class ListProductsControllerTests : IDisposable
{
    private readonly Fixture _fixture = new();
    private readonly ListProductsController _controller;
    private readonly ApplicationDbContext _dbContext;

    public ListProductsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("app.db")
            .Options;

        var db = new ApplicationDbContext(options);

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        _dbContext = db;

        _controller = new ListProductsController(_dbContext);
    }

    [Fact]
    public async Task ListProducts_ShouldReturnAllProducts_WhenNoPageTokenProvided()
    {
        _dbContext.Products.AddRange(_fixture.CreateMany<DomainModels.Product>(20));
        await _dbContext.SaveChangesAsync();
        
        var request = new ListProductsRequest { MaxPageSize = 4 };

        var result = await _controller.ListProducts(request);
        
        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listProductsResponse = response.Value as ListProductsResponse;
        listProductsResponse!.Results.Count().ShouldBe(4);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}