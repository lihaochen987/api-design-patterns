using AutoFixture;
using backend.Database;
using backend.Product.Controllers;
using backend.Product.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

[Collection("SequentialExecutionCollection")]
public class ListProductsControllerTests : IDisposable
{
    private readonly ListProductsController _controller;
    private readonly ApplicationDbContext _dbContext;
    private const int DefaultMaxPageSize = 10;

    public ListProductsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=localhost;Database=mytestdatabase;Username=myusername;Password=mypassword")
            .Options;

        var db = new ApplicationDbContext(options);
        db.Database.EnsureCreated();
        _dbContext = db;
        var extensions = new GetProductExtensions();
        _controller = new ListProductsController(_dbContext, extensions);
    }

    [Fact]
    public async Task ListProducts_ShouldReturnAllProducts_WhenNoPageTokenProvided()
    {
        var products = new ProductTestDataBuilder().CreateMany(4);
        _dbContext.Products.AddRange(products);
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

    [Fact]
    public async Task ListProducts_ShouldReturnProductsAfterPageToken_WhenPageTokenProvided()
    {
        var products = new ProductTestDataBuilder().CreateMany(4);
        _dbContext.Products.AddRange(products);
        await _dbContext.SaveChangesAsync();

        var request = new ListProductsRequest { PageToken = "2", MaxPageSize = 2 };

        var result = await _controller.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listProductsResponse = response.Value as ListProductsResponse;
        listProductsResponse!.Results.Count().ShouldBe(2);
        listProductsResponse.Results.First()!.Id.ShouldBe("3");
        listProductsResponse.Results.Last()!.Id.ShouldBe("4");
        listProductsResponse.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task ListProducts_ShouldReturnNextPageToken_WhenMoreProductsExist()
    {
        var products = new ProductTestDataBuilder().CreateMany(20);
        _dbContext.Products.AddRange(products);
        await _dbContext.SaveChangesAsync();

        var request = new ListProductsRequest { MaxPageSize = 2 };

        var result = await _controller.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listProductsResponse = response.Value as ListProductsResponse;
        listProductsResponse!.Results.Count().ShouldBe(2);
        listProductsResponse.NextPageToken.ShouldBeEquivalentTo("2");
    }

    [Fact]
    public async Task ListProducts_ShouldUseDefaults_WhenPageTokenAndMaxPageSizeNotProvided()
    {
        var products = new ProductTestDataBuilder().CreateMany(20);
        _dbContext.Products.AddRange(products);
        await _dbContext.SaveChangesAsync();

        var request = new ListProductsRequest();

        var result = await _controller.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listProductsResponse = response.Value as ListProductsResponse;
        listProductsResponse!.Results.Count().ShouldBe(DefaultMaxPageSize);
        listProductsResponse.NextPageToken.ShouldBeEquivalentTo(DefaultMaxPageSize.ToString());
    }

    [Fact]
    public async Task ListProducts_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        var request = new ListProductsRequest { MaxPageSize = 2 };

        var result = await _controller.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listProductsResponse = response.Value as ListProductsResponse;
        listProductsResponse!.Results.ShouldBeEmpty();
        listProductsResponse.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task ListProducts_ShouldReturnCategoryAsString()
    {
        var products = new ProductTestDataBuilder().CreateMany(20);
        _dbContext.Products.AddRange(products);
        await _dbContext.SaveChangesAsync();

        var request = new ListProductsRequest();

        var result = await _controller.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listProductsResponse = response.Value as ListProductsResponse;
        listProductsResponse!.Results.First()!.Category.ShouldBeOfType(typeof(string));
        listProductsResponse.Results.Count().ShouldBe(DefaultMaxPageSize);
        listProductsResponse.NextPageToken.ShouldBeEquivalentTo(DefaultMaxPageSize.ToString());
    }

    [Fact]
    public async Task ListProducts_WithFilter_ReturnsFilteredResults()
    {
        var product = new ProductTestDataBuilder().WithPrice(15).WithCategory(Category.DogFood).Build();
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var request = new ListProductsRequest
        {
            Filter = "Category == \"DogFood\" && Price < 20",
            MaxPageSize = 10
        };

        var result = await _controller.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listProductsResponse = response.Value as ListProductsResponse;
        listProductsResponse!.Results.Count().ShouldBe(1);
        listProductsResponse.Results
            .All(p => p!.Category == product.Category.ToString() && int.Parse(p.Price) < 20)
            .ShouldBeTrue();
    }

    [Fact]
    public async Task ListProducts_WithFilterAndPagination_ReturnsCorrectResults()
    {
        var product = new ProductTestDataBuilder().WithId(2).WithPrice(15).WithCategory(Category.Beds).Build();
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var request = new ListProductsRequest
        {
            Filter = "Category == \"Beds\"",
            MaxPageSize = 2,
            PageToken = "1"
        };

        var result = await _controller.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listProductsResponse = response.Value as ListProductsResponse;
        listProductsResponse!.Results.Count().ShouldBe(1);
        listProductsResponse.Results
            .All(p => p!.Category == product.Category.ToString())
            .ShouldBeTrue();
    }

    [Fact]
    public async Task ListProducts_WithFilterAndPagination_ReturnsCorrectResultsWhenScattered()
    {
        var firstFilteredProduct = new ProductTestDataBuilder().WithId(1).WithCategory(Category.Beds).Build();
        var products = new ProductTestDataBuilder().WithCategory(Category.Clothing).CreateMany(10, 2);
        var secondFilteredProducts = new ProductTestDataBuilder().WithId(12).WithCategory(Category.Beds).Build();
        _dbContext.Products.AddRange(products);
        _dbContext.Products.AddRange(firstFilteredProduct, secondFilteredProducts);
        await _dbContext.SaveChangesAsync();

        var request = new ListProductsRequest
        {
            Filter = "Category == \"Beds\"",
            MaxPageSize = 5,
            PageToken = "1"
        };

        var result = await _controller.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listProductsResponse = response.Value as ListProductsResponse;
        listProductsResponse!.Results.Count().ShouldBe(2);
    }

    [Fact]
    public async Task ListProducts_WithInvalidFilter_ReturnsBadRequest()
    {
        var request = new ListProductsRequest
        {
            Filter = "InvalidFilter == \"NonExistent\"",
            MaxPageSize = 10
        };

        await _controller.ListProducts(request).ShouldThrowAsync<ArgumentException>();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}