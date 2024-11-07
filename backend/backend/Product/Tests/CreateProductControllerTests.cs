using AutoFixture;
using backend.Database;
using backend.Product.Controllers;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

[Collection("SequentialExecutionCollection")]
public class CreateProductControllerTests : IDisposable
{
    private readonly CreateProductController _controller;
    private readonly ApplicationDbContext _dbContext;
    private readonly CreateProductExtensions _extensions;

    public CreateProductControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=localhost;Database=mytestdatabase;Username=myusername;Password=mypassword")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureCreated();
        _extensions = new CreateProductExtensions(new TypeParser());
        _controller = new CreateProductController(_dbContext, _extensions);
    }

    [Fact]
    public async Task CreateProduct_Should_AddProduct_And_ReturnCreatedAtActionResult()
    {
        var product = new ProductTestDataBuilder().Build();
        var request = _extensions.ToCreateProductRequest(product);
        var expectedResponse = _extensions.ToCreateProductResponse(product);

        var result = await _controller.CreateProduct(request);

        var createdAtActionResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
        createdAtActionResult.ActionName.ShouldBe("GetProduct");
        createdAtActionResult.ControllerName.ShouldBe("GetProduct");
        var actualResponse = createdAtActionResult.Value.ShouldBeOfType<CreateProductResponse>();
        actualResponse.ShouldBeEquivalentTo(expectedResponse);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}