using System.Globalization;
using AutoFixture;
using backend.Database;
using backend.Product.Contracts;
using backend.Product.Controllers;
using backend.Product.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

[Collection("SequentialExecutionCollection")]
public class ReplaceProductControllerTests
{
    private readonly Fixture _fixture = new();
    private readonly ReplaceProductController _controller;
    private readonly ApplicationDbContext _dbContext;

    public ReplaceProductControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite($"Filename=:memory:{Guid.NewGuid()};Mode=Memory;Cache=Shared")
            .Options;

        var db = new ApplicationDbContext(options);

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        _dbContext = db;
        _controller = new ReplaceProductController(_dbContext);
    }

    [Fact]
    public async Task ReplaceProduct_Should_ReturnOk_WithUpdatedProduct_When_ProductExists()
    {
        var originalProduct = _fixture.Create<DomainModels.Product>();
        _dbContext.Products.Add(originalProduct);
        await _dbContext.SaveChangesAsync();

        var request = new ReplaceProductRequest
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

        var result = await _controller.ReplaceProduct(originalProduct.Id, request);

        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response!.Value.ShouldBeOfType<ReplaceProductResponse>();
        var replaceProductResponse = response.Value as ReplaceProductResponse;
        replaceProductResponse.ShouldBeEquivalentTo(originalProduct.ToReplaceProductResponse());
        var updatedProduct = await _dbContext.Products.FindAsync(originalProduct.Id);
        updatedProduct.ShouldNotBeNull();
        updatedProduct.ShouldBeEquivalentTo(originalProduct);
    }
}