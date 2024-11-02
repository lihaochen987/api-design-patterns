using System.Globalization;
using AutoFixture;
using backend.Database;
using backend.Product.Contracts;
using backend.Product.Controllers;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Category = backend.Product.DomainModels.Category;

namespace backend.Product.Tests;

[Collection("SequentialExecutionCollection")]
public class CreateProductControllerTests : IDisposable
{
    private readonly Fixture _fixture = new();
    private readonly CreateProductController _controller;
    private readonly ApplicationDbContext _dbContext;
    private readonly CreateProductExtensions _extensions;

    public CreateProductControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=localhost;Database=mytestdatabase;Username=myusername;Password=mypassword")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureCreated(); // Ensure the database schema is created
        _extensions = new CreateProductExtensions(new TypeParser());
        _controller = new CreateProductController(_dbContext, _extensions);
    }

    [Fact]
    public async Task CreateProduct_Should_AddProduct_And_ReturnCreatedAtActionResult()
    {
        var request = new CreateProductRequest
        {
            Name = _fixture.Create<string>(),
            Price = _fixture.Create<decimal>().ToString(CultureInfo.InvariantCulture),
            Category = _fixture.Create<Category>().ToString(),
            Dimensions = new DimensionsContract
            {
                Length = _fixture.Create<decimal>().ToString(CultureInfo.InvariantCulture),
                Width = _fixture.Create<decimal>().ToString(CultureInfo.InvariantCulture),
                Height = _fixture.Create<decimal>().ToString(CultureInfo.InvariantCulture)
            }
        };
        var expectedProduct = _extensions.ToEntity(request);
        var expectedResponse = _extensions.ToCreateProductResponse(expectedProduct);

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