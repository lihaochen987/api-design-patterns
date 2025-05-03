using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

// Todo: Refactor some tests in this file
public class ListProductsControllerTests : ListProductsControllerTestBase
{
    [Fact]
    public async Task ListProducts_ShouldReturnAllProducts_WhenNoPageTokenProvided()
    {
        List<ProductView> productViews = new ProductViewTestDataBuilder().CreateMany(4).ToList();
        ListProductsRequest request = new() { MaxPageSize = 4 };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(productViews, null, 10));
        ListProductsController sut = ListProductsController();
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });

        var result = await sut.ListProducts(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.Results.Should().HaveCount(4);
        listProductsResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListProducts_ShouldReturnCachedResponse_WhenValidCacheExists()
    {
        ListProductsRequest request = new() { MaxPageSize = 4 };
        var cachedResponse = new ListProductsResponse
        {
            Results = Fixture.CreateMany<GetProductResponse>(), NextPageToken = null
        };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = cachedResponse, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });

        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        response.Value.Should().Be(cachedResponse);
    }

    [Fact]
    public async Task ListProducts_ShouldReturnProductsAfterPageToken_WhenPageTokenProvided()
    {
        List<ProductView> productViewList = new ProductViewTestDataBuilder().CreateMany(4).ToList();
        var expectedPageResults = productViewList.Skip(2).Take(2).ToList();
        ListProductsRequest request = new() { PageToken = "2", MaxPageSize = 2 };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(expectedPageResults, null, 10));
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.Results.Should().HaveCount(2);
        listProductsResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListProducts_ShouldReturnNextPageToken_WhenMoreProductsExist()
    {
        List<ProductView> products = new ProductViewTestDataBuilder().CreateMany(20).ToList();
        List<ProductView> firstPageProducts = products.Take(2).ToList();
        ListProductsRequest request = new() { MaxPageSize = 2 };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(firstPageProducts, "2", 10));
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.Results.Should().HaveCount(2);
        listProductsResponse.NextPageToken.Should().BeEquivalentTo("2");
    }

    [Fact]
    public async Task ListProducts_ShouldUseDefaults_WhenPageTokenAndMaxPageSizeNotProvided()
    {
        List<ProductView> products = new ProductViewTestDataBuilder().CreateMany(20).ToList();
        List<ProductView> defaultPageProducts = products.Take(DefaultMaxPageSize).ToList();
        ListProductsRequest request = new();
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(defaultPageProducts, DefaultMaxPageSize.ToString(), 10));
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.Results.Should().HaveCount(DefaultMaxPageSize);
        listProductsResponse.NextPageToken.Should().BeEquivalentTo(DefaultMaxPageSize.ToString());
    }

    [Fact]
    public async Task ListProducts_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        ListProductsRequest request = new() { MaxPageSize = 2 };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts([], null, 10));
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.Results.Should().BeEmpty();
        listProductsResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListProducts_ShouldReturnCategoryAsString()
    {
        List<ProductView> products = new ProductViewTestDataBuilder().CreateMany(20).ToList();
        List<ProductView> firstPageProducts = products.Take(DefaultMaxPageSize).ToList();
        ListProductsRequest request = new();
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(firstPageProducts, DefaultMaxPageSize.ToString(), 10));
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.Should().NotBeNull();
        listProductsResponse.Results.Should().HaveCount(DefaultMaxPageSize);
        listProductsResponse.NextPageToken.Should().BeEquivalentTo(DefaultMaxPageSize.ToString());
    }


    [Fact]
    public async Task ListProducts_WithCalculatedFieldFilter_ReturnsFilteredResults()
    {
        var product = new ProductViewTestDataBuilder().WithCategory(Category.PetFood).WithPrice(15).Build();
        var filteredProducts = new List<ProductView> { product };
        ListProductsRequest request = new() { Filter = "Category == \"PetFood\" && Price < 20", MaxPageSize = 10 };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(filteredProducts, null, 10));
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        var response = (OkObjectResult)result.Result!;
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.Should().NotBeNull();

        var singleItem = listProductsResponse.Results.Should().ContainSingle().Subject;
        using (new AssertionScope())
        {
            ((GetPetFoodResponse)singleItem).Category.Should().Be(product.Category);
            decimal.Parse(((GetPetFoodResponse)singleItem).Price).Should().BeLessThan(20);
        }
    }

    [Fact]
    public async Task ListProducts_WithNestedFieldFilter_ReturnsFilteredResults()
    {
        var product = new ProductViewTestDataBuilder().WithDimensions(new Dimensions(5, 1, 30)).Build();
        var filteredProducts = new List<ProductView> { product };
        ListProductsRequest request = new()
        {
            Filter = "Dimensions.Length == 5 && Dimensions.Width < 20", MaxPageSize = 10
        };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(filteredProducts, null, 10));
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        var response = (OkObjectResult)result.Result!;
        var listProductsResponse = (ListProductsResponse)response.Value!;

        GetProductResponse? singleItem = listProductsResponse.Results.Should().ContainSingle().Subject;
        using (new AssertionScope())
        {
            int.Parse(singleItem.Dimensions.Length).Should().Be(5);
            int.Parse(singleItem.Dimensions.Width).Should().BeLessThan(20);
        }
    }


    [Fact]
    public async Task ListProducts_WithSpacedFieldFilter_ReturnsFilteredResults()
    {
        var product = new ProductViewTestDataBuilder().WithName("Chew Toy").Build();
        var filteredProducts = new List<ProductView> { product };
        ListProductsRequest request = new() { Filter = "Name == \"Chew Toy\"", MaxPageSize = 10 };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(filteredProducts, null, 10));
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        var response = (OkObjectResult)result.Result!;
        response.Should().NotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.Should().NotBeNull();
        GetProductResponse? singleItem = listProductsResponse.Results.Should().ContainSingle().Subject;
        singleItem.Name.Should().Be(product.Name);
    }


    [Fact]
    public async Task ListProducts_WithFilterAndPagination_ReturnsCorrectResults()
    {
        var product = new ProductViewTestDataBuilder().WithId(2).WithCategory(Category.Beds).Build();
        var filteredProducts = new List<ProductView> { product };
        ListProductsRequest request = new() { Filter = "Category == \"Beds\"", MaxPageSize = 2, PageToken = "1" };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(filteredProducts, "2", 10));
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.Results.Should().HaveCount(1);
        listProductsResponse.Results
            .All(p => p.Category == product.Category)
            .Should().BeTrue();
    }

    [Fact]
    public async Task ListProducts_WithFilterAndPagination_ReturnsCorrectResultsWhenScattered()
    {
        var firstFilteredProduct = new ProductViewTestDataBuilder().WithId(1).WithCategory(Category.Beds).Build();
        var secondFilteredProduct = new ProductViewTestDataBuilder().WithId(12).WithCategory(Category.Beds).Build();
        var filteredProducts = new List<ProductView> { firstFilteredProduct, secondFilteredProduct };
        ListProductsRequest request = new() { Filter = "Category == \"Beds\"", MaxPageSize = 5, PageToken = "1" };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(filteredProducts, null, 10));
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new GetListProductsFromCacheResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = CacheKey
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.Results.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListProducts_WithInvalidFilter_ReturnsBadRequest()
    {
        ListProductsRequest request = new() { Filter = "InvalidFilter == \"NonExistent\"", MaxPageSize = 10 };
        MockQueryProcessor
            .Setup(svc => svc.Process(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ThrowsAsync(new ArgumentException("Invalid filter syntax"));

        ListProductsController sut = ListProductsController();

        Func<Task> act = async () => await sut.ListProducts(request);
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
