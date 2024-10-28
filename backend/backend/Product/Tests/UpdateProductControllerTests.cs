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
public class UpdateProductControllerTests
{
    private readonly Fixture _fixture = new();
    private readonly UpdateProductController _controller;
    private readonly ApplicationDbContext _dbContext;

    public UpdateProductControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite($"Filename=:memory:{Guid.NewGuid()};Mode=Memory;Cache=Shared")
            .Options;

        var db = new ApplicationDbContext(options);
        db.Database.EnsureCreated();
        _dbContext = db;
        _controller = new UpdateProductController(_dbContext);
    }

    [Fact]
    public async Task UpdateProduct_WithValidFieldMask_ShouldUpdateSpecifiedFields()
    {
        var product = new DomainModels.Product(
            2,
            _fixture.Create<string>(),
            _fixture.Create<decimal>(),
            Category.Beds,
            _fixture.Create<Dimensions>());
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateProductRequest
        {
            Name = "Updated Name",
            FieldMask = ["name"]
        };

        var actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        var response = contentResult.Value as UpdateProductResponse;
        response!.Name.ShouldBeEquivalentTo(request.Name);
    }

    [Fact]
    public async Task UpdateProduct_NonExistentProduct_ShouldReturnNotFound()
    {
        var request = new UpdateProductRequest
        {
            Name = "Non-Existent Product",
            FieldMask = ["name"]
        };
        const int nonExistentId = 999;

        var actionResult = await _controller.UpdateProduct(nonExistentId, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateProduct_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        const string originalName = "Original Name";
        const decimal originalPrice = 20.99m;
        var product = new DomainModels.Product(
            3,
            originalName,
            originalPrice,
            Category.Feeders,
            _fixture.Create<Dimensions>());
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateProductRequest
        {
            Name = "Updated Name",
            Price = "25.50",
            FieldMask = ["name", "price"]
        };

        var actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        var response = contentResult.Value as UpdateProductResponse;
        response!.Name.ShouldBeEquivalentTo(request.Name);
        response.Price.ShouldBeEquivalentTo(request.Price);
    }

    [Fact]
    public async Task UpdateProduct_WithPartialFieldMask_ShouldNotUpdateUnspecifiedFields()
    {
        const Category originalCategory = Category.Toys;
        var product = new DomainModels.Product(
            4,
            "Original Name",
            15.75m,
            originalCategory,
            _fixture.Create<Dimensions>());
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateProductRequest
        {
            Name = "Updated Name",
            FieldMask = ["name"]
        };

        var actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        var response = contentResult.Value as UpdateProductResponse;
        response!.Name.ShouldBeEquivalentTo(request.Name);
        response.Category.ShouldBe(originalCategory.ToString());
    }

    [Fact]
    public async Task UpdateProduct_WithNestedFieldInFieldMask_ShouldUpdateNestedField()
    {
        var product = new DomainModels.Product(
            5,
            "Original Product",
            35.00m,
            Category.CollarsAndLeashes,
            new Dimensions(10, 5, 2));
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateProductRequest
        {
            Dimensions = new DimensionsContract { Length = "20", Width = "10", Height = "2" },
            FieldMask = ["dimensions.width", "dimensions.height"]
        };

        var actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        var response = contentResult.Value as UpdateProductResponse;
        response!.Dimensions.Length.ShouldBe(product.Dimensions.Length.ToString(CultureInfo.InvariantCulture));
        response.Dimensions.Width.ShouldBe(request.Dimensions.Width);
        response.Dimensions.Height.ShouldBe(request.Dimensions.Height);
    }
}