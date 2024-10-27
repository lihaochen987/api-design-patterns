using AutoFixture;
using backend.Database;
using backend.Parsers;
using backend.Parsers.CelSpecParsing;
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
    private readonly Fixture _fixture = new();
    private readonly ListProductsController _controller;
    private readonly ApplicationDbContext _dbContext;
    private const int DefaultMaxPageSize = 10;

    public ListProductsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite($"Filename=:memory:{Guid.NewGuid()};Mode=Memory;Cache=Shared")
            .Options;

        var db = new ApplicationDbContext(options);

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        _dbContext = db;

        var celOperators = new CelOperators();
        _controller = new ListProductsController(_dbContext, celOperators);
    }

    [Fact]
    public async Task ListProducts_ShouldReturnAllProducts_WhenNoPageTokenProvided()
    {
        _dbContext.Products.AddRange(_fixture.CreateMany<DomainModels.Product>(4));
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
        _dbContext.Products.AddRange(_fixture.CreateMany<DomainModels.Product>(4));
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
        _dbContext.Products.AddRange(_fixture.CreateMany<DomainModels.Product>(20));
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
        _dbContext.Products.AddRange(_fixture.CreateMany<DomainModels.Product>(20));
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
        _dbContext.Products.AddRange(_fixture.CreateMany<DomainModels.Product>(20));
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
        var product = new DomainModels.Product(
            1,
            _fixture.Create<string>(),
            15,
            Category.DogFood,
            _fixture.Create<Dimensions>());
        _dbContext.Products.AddRange(product);
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

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}