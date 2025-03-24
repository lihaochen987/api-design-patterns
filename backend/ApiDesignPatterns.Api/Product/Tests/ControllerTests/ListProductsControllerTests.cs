using AutoFixture;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class ListProductsControllerTests : ListProductsControllerTestBase
{
    [Fact]
    public async Task ListProducts_ShouldReturnAllProducts_WhenNoPageTokenProvided()
    {
        List<ProductView> productViews = new ProductViewTestDataBuilder().CreateMany(4).ToList();
        ListProductsRequest request = new() { MaxPageSize = 4 };
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(productViews, null, 10));
        ListProductsController sut = ListProductsController();
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });

        var result = await sut.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse!.Results.Count().ShouldBe(4);
        listProductsResponse.NextPageToken.ShouldBeNull();
        Mock.Get(MockPersistListProductsToCache)
            .Verify(svc => svc.Handle(It.Is<PersistListProductsToCacheCommand>(c =>
                    c.CacheKey == "products-cache-key" &&
                    c.Products == listProductsResponse)),
                Times.Once);
    }

    [Fact]
    public async Task ListProducts_ShouldReturnCachedResponse_WhenValidCacheExists()
    {
        ListProductsRequest request = new() { MaxPageSize = 4 };
        var cachedResponse = new ListProductsResponse
        {
            Results = Fixture.CreateMany<GetProductResponse>(), NextPageToken = null
        };
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = cachedResponse,
                SelectedForStalenessCheck = false,
                CacheKey = "products-cache-key"
            });

        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        response.Value.ShouldBe(cachedResponse);
        Mock.Get(MockListProducts)
            .Verify(svc => svc.Handle(It.IsAny<ListProductsQuery>()), Times.Never);
    }

    [Fact]
    public async Task ListProducts_ShouldReturnProductsAfterPageToken_WhenPageTokenProvided()
    {
        List<ProductView> productViewList = new ProductViewTestDataBuilder().CreateMany(4).ToList();
        var expectedPageResults = productViewList.Skip(2).Take(2).ToList();
        ListProductsRequest request = new() { PageToken = "2", MaxPageSize = 2 };
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(expectedPageResults, null, 10));
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse!.Results.Count().ShouldBe(2);
        listProductsResponse.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task ListProducts_ShouldReturnNextPageToken_WhenMoreProductsExist()
    {
        List<ProductView> products = new ProductViewTestDataBuilder().CreateMany(20).ToList();
        List<ProductView> firstPageProducts = products.Take(2).ToList();
        ListProductsRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(firstPageProducts, "2", 10));
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse!.Results.Count().ShouldBe(2);
        listProductsResponse.NextPageToken.ShouldBeEquivalentTo("2");
    }

    [Fact]
    public async Task ListProducts_ShouldUseDefaults_WhenPageTokenAndMaxPageSizeNotProvided()
    {
        List<ProductView> products = new ProductViewTestDataBuilder().CreateMany(20).ToList();
        List<ProductView> defaultPageProducts = products.Take(DefaultMaxPageSize).ToList();
        ListProductsRequest request = new();
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(defaultPageProducts, DefaultMaxPageSize.ToString(), 10));
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse!.Results.Count().ShouldBe(DefaultMaxPageSize);
        listProductsResponse.NextPageToken.ShouldBeEquivalentTo(DefaultMaxPageSize.ToString());
    }

    [Fact]
    public async Task ListProducts_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        ListProductsRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts([], null, 10));
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse!.Results.ShouldBeEmpty();
        listProductsResponse.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task ListProducts_ShouldReturnCategoryAsString()
    {
        List<ProductView> products = new ProductViewTestDataBuilder().CreateMany(20).ToList();
        List<ProductView> firstPageProducts = products.Take(DefaultMaxPageSize).ToList();
        ListProductsRequest request = new();
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(firstPageProducts, DefaultMaxPageSize.ToString(), 10));
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.ShouldNotBeNull();
        listProductsResponse.Results.Count().ShouldBe(DefaultMaxPageSize);
        listProductsResponse.NextPageToken.ShouldBeEquivalentTo(DefaultMaxPageSize.ToString());
    }


    [Fact]
    public async Task ListProducts_WithCalculatedFieldFilter_ReturnsFilteredResults()
    {
        var product = new ProductViewTestDataBuilder().WithCategory(Category.PetFood).WithPrice(15).Build();
        var filteredProducts = new List<ProductView> { product };
        ListProductsRequest request = new() { Filter = "Category == \"PetFood\" && Price < 20", MaxPageSize = 10 };
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(filteredProducts, null, 10));
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        var response = (OkObjectResult)result.Result!;
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.ShouldNotBeNull();
        listProductsResponse.Results.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => ((GetPetFoodResponse)p).Category.ShouldBe(product.Category),
                p => decimal.Parse(((GetPetFoodResponse)p).Price).ShouldBeLessThan(20));
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
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(filteredProducts, null, 10));
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        var response = (OkObjectResult)result.Result!;
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse!.Results.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => int.Parse(p.Dimensions.Length).ShouldBe(5),
                p => int.Parse(p.Dimensions.Width).ShouldBeLessThan(20));
    }


    [Fact]
    public async Task ListProducts_WithSpacedFieldFilter_ReturnsFilteredResults()
    {
        var product = new ProductViewTestDataBuilder().WithName("Chew Toy").Build();
        var filteredProducts = new List<ProductView> { product };
        ListProductsRequest request = new() { Filter = "Name == \"Chew Toy\"", MaxPageSize = 10 };
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(filteredProducts, null, 10));
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        var response = (OkObjectResult)result.Result!;
        response.ShouldNotBeNull();
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse.ShouldNotBeNull();
        listProductsResponse.Results.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.Name.ShouldBe(product.Name));
    }


    [Fact]
    public async Task ListProducts_WithFilterAndPagination_ReturnsCorrectResults()
    {
        var product = new ProductViewTestDataBuilder().WithId(2).WithCategory(Category.Beds).Build();
        var filteredProducts = new List<ProductView> { product };
        ListProductsRequest request = new() { Filter = "Category == \"Beds\"", MaxPageSize = 2, PageToken = "1" };
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(filteredProducts, "2", 10));
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse!.Results.Count().ShouldBe(1);
        listProductsResponse.Results
            .All(p => p.Category == product.Category)
            .ShouldBeTrue();
    }

    [Fact]
    public async Task ListProducts_WithFilterAndPagination_ReturnsCorrectResultsWhenScattered()
    {
        var firstFilteredProduct = new ProductViewTestDataBuilder().WithId(1).WithCategory(Category.Beds).Build();
        var secondFilteredProduct = new ProductViewTestDataBuilder().WithId(12).WithCategory(Category.Beds).Build();
        var filteredProducts = new List<ProductView> { firstFilteredProduct, secondFilteredProduct };
        ListProductsRequest request = new() { Filter = "Category == \"Beds\"", MaxPageSize = 5, PageToken = "1" };
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedProducts(filteredProducts, null, 10));
        Mock
            .Get(MockGetListProductsFromCache)
            .Setup(svc => svc.Handle(It.Is<GetListProductsFromCacheQuery>(q =>
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.Request.Filter == request.Filter)))
            .ReturnsAsync(new CacheQueryResult
            {
                ProductsResponse = null, SelectedForStalenessCheck = false, CacheKey = "products-cache-key"
            });
        ListProductsController sut = ListProductsController();

        var result = await sut.ListProducts(request);

        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        var listProductsResponse = (ListProductsResponse)response.Value!;
        listProductsResponse!.Results.Count().ShouldBe(2);
    }

    [Fact]
    public async Task ListProducts_WithInvalidFilter_ReturnsBadRequest()
    {
        ListProductsRequest request = new() { Filter = "InvalidFilter == \"NonExistent\"", MaxPageSize = 10 };
        Mock
            .Get(MockListProducts)
            .Setup(svc => svc.Handle(It.Is<ListProductsQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ThrowsAsync(new ArgumentException("Invalid filter syntax"));

        ListProductsController sut = ListProductsController();

        await sut.ListProducts(request).ShouldThrowAsync<ArgumentException>();
    }
}
