using System.Globalization;
using backend.Database;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Product.ProductControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

[Collection("SequentialExecutionCollection")]
public class UpdateProductControllerTests
{
    private readonly UpdateProductController _controller;
    private readonly ApplicationDbContext _dbContext;
    private readonly UpdateProductExtensions _extensions;

    public UpdateProductControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=localhost;Database=mytestdatabase;Username=myusername;Password=mypassword")
            .Options;

        var db = new ApplicationDbContext(options);
        db.Database.EnsureCreated();
        _dbContext = db;
        var configuration = new ProductFieldMaskConfiguration();
        _extensions = new UpdateProductExtensions();
        _controller = new UpdateProductController(
            _dbContext,
            configuration,
            _extensions);
    }

    [Fact]
    public async Task UpdateProduct_WithEmptyFieldMask_ShouldUpdateNoFields()
    {
        var product = new ProductTestDataBuilder().Build();
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateProductRequest
        {
            Name = "Updated Name",
            Price = "1.99M",
            Category = "Toys"
        };

        var actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        var response = contentResult.Value as UpdateProductResponse;
        response.ShouldBeEquivalentTo(_extensions.ToUpdateProductResponse(product));
    }

    [Fact]
    public async Task UpdateProduct_WithValidFieldMask_ShouldUpdateSpecifiedFields()
    {
        var product = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
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
        var product = new ProductTestDataBuilder().WithId(3).WithName(originalName).WithPrice(originalPrice)
            .WithCategory(Category.Feeders).Build();
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateProductRequest
        {
            Name = "Updated Name",
            Price = "25.50",
            Category = "Toys",
            FieldMask = ["name", "category"]
        };

        var actionResult = await _controller.UpdateProduct(product.Id, request);

        var response = Assert.IsType<OkObjectResult>(actionResult.Result).Value as UpdateProductResponse;
        response.ShouldNotBeNull();
        response.Name.ShouldBeEquivalentTo(request.Name);
        response.Category.ShouldBeEquivalentTo(request.Category);
    }

    [Fact]
    public async Task UpdateProduct_WithNestedFieldInFieldMask_ShouldUpdateNestedField()
    {
        var product = new ProductTestDataBuilder()
            .WithId(5).WithDimensions(new Dimensions(10, 5, 2)).Build();
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