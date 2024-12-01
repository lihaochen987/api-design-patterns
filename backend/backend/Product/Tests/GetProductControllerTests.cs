using System.Globalization;
using AutoFixture;
using AutoMapper;
using backend.Product.FieldMasks;
using backend.Product.ProductControllers;
using backend.Product.Tests.Builders;
using backend.Product.Tests.Fakes;
using backend.Product.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

public class GetProductControllerTests
{
    private readonly GetProductController _controller;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();
    private readonly ProductViewRepositoryFake _productRepository = [];

    public GetProductControllerTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<GetProductMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
        ProductFieldMaskConfiguration configuration = new();
        _controller = new GetProductController(
            _productRepository,
            configuration,
            _mapper);
    }

    [Fact]
    public async Task GetProduct_ReturnsFullProduct_WhenFieldMaskIsWildcard()
    {
        ProductView product = new ProductViewTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductRequest? request = _fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["*"])
            .Create();

        ActionResult<GetProductResponse> actionResult = await _controller.GetProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        GetProductResponse? response =
            JsonConvert.DeserializeObject<GetProductResponse>(contentResult.Value!.ToString()!);
        response.ShouldBeEquivalentTo(_mapper.Map<GetProductResponse>(product));
    }

    [Fact]
    public async Task GetProduct_ReturnsFullProduct_WhenFieldMaskIsEmpty()
    {
        ProductView product = new ProductViewTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductRequest? request = _fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, [])
            .Create();

        ActionResult<GetProductResponse> actionResult = await _controller.GetProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        GetProductResponse? response =
            JsonConvert.DeserializeObject<GetProductResponse>(contentResult.Value!.ToString()!);
        response.ShouldBeEquivalentTo(_mapper.Map<GetProductResponse>(product));
    }

    [Fact]
    public async Task GetProduct_DefaultsToWildCard_WhenOnlyFieldMaskIsNotMatched()
    {
        ProductView product = new ProductViewTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductRequest? request = _fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["UnmatchedField"])
            .Create();

        ActionResult<GetProductResponse> actionResult = await _controller.GetProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        GetProductResponse? response =
            JsonConvert.DeserializeObject<GetProductResponse>(contentResult.Value!.ToString()!);
        response.ShouldBeEquivalentTo(_mapper.Map<GetProductResponse>(product));
    }

    [Fact]
    public async Task GetProduct_ReturnsValidMasks_WhenInvalidMasksArePassed()
    {
        ProductView product = new ProductViewTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductRequest? request = _fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["UnmatchedField", "Price", "Name"])
            .Create();

        ActionResult<GetProductResponse> actionResult = await _controller.GetProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        Dictionary<string, object>? response =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(contentResult.Value!.ToString()!);
        response!.Count.ShouldBeEquivalentTo(2);
        response.ShouldContainKeyAndValue("Name", product.Name);
        response.ShouldContainKeyAndValue("Price", product.Price.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public async Task GetProduct_ReturnsPartialProduct_WhenFieldMaskIsSpecified()
    {
        ProductView product = new ProductViewTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductRequest? request = _fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["name"])
            .Create();

        ActionResult<GetProductResponse> actionResult = await _controller.GetProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? result = actionResult.Result as OkObjectResult;
        Dictionary<string, object>? response =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(result!.Value!.ToString()!);
        response!.ShouldContainKey("Name");
        response!.Count.ShouldBeEquivalentTo(1);
    }

    [Fact]
    public async Task GetProduct_ReturnsPartialProduct_WhenNestedFieldMaskIsSpecified()
    {
        ProductView product = new ProductViewTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductRequest? request = _fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["dimensions.width"])
            .Create();

        ActionResult<GetProductResponse> actionResult = await _controller.GetProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? result = actionResult.Result as OkObjectResult;
        Dictionary<string, object>? response =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(result!.Value!.ToString()!);
        response!.ShouldContainKey("Dimensions");
        JObject? dimensions = response!["Dimensions"] as JObject;
        dimensions!.ShouldContainKey("Width");
        dimensions!.Count.ShouldBeEquivalentTo(1);
    }

    [Fact]
    public async Task GetProduct_ReturnsAllFields_WhenPartialRequestIsMade()
    {
        ProductView product = new ProductViewTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductRequest? request = _fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["dimensions.*"])
            .Create();

        ActionResult<GetProductResponse> actionResult = await _controller.GetProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? result = actionResult.Result as OkObjectResult;
        Dictionary<string, object>? response =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(result!.Value!.ToString()!);
        response!.ShouldContainKey("Dimensions");
        JObject? dimensions = response!["Dimensions"] as JObject;
        dimensions!.Count.ShouldBeEquivalentTo(3);
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        GetProductRequest? request = _fixture.Create<GetProductRequest>();

        ActionResult<GetProductResponse> result = await _controller.GetProduct(999, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
    }
}
