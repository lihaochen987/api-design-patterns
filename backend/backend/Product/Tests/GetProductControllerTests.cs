using System.Globalization;
using AutoFixture;
using backend.Product.FieldMasks;
using backend.Product.ProductControllers;
using backend.Product.Tests.Builders;
using backend.Product.Tests.Fakes;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace backend.Product.Tests
{
    public class GetProductControllerTests
    {
        private readonly Fixture _fixture = new();
        private readonly GetProductController _controller;
        private readonly GetProductExtensions _extensions;
        private readonly ProductViewRepositoryFake _productRepository = [];

        public GetProductControllerTests()
        {
            var configuration = new ProductFieldMaskConfiguration();
            _extensions = new GetProductExtensions(new TypeParser());
            _controller = new GetProductController(
                _productRepository,
                configuration,
                _extensions);
        }

        [Fact]
        public async Task GetProduct_ReturnsFullProduct_WhenFieldMaskIsWildcard()
        {
            var product = new ProductViewTestDataBuilder().Build();
            _productRepository.Add(product);

            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, ["*"])
                .Create();

            var actionResult = await _controller.GetProduct(product.Id, request);

            actionResult.Result.ShouldNotBeNull();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            var contentResult = actionResult.Result as OkObjectResult;
            contentResult.ShouldNotBeNull();
            var response = JsonConvert.DeserializeObject<GetProductResponse>(contentResult.Value!.ToString()!);
            response.ShouldBeEquivalentTo(_extensions.ToGetProductResponse(product));
        }

        [Fact]
        public async Task GetProduct_ReturnsFullProduct_WhenFieldMaskIsEmpty()
        {
            var product = new ProductViewTestDataBuilder().Build();
            _productRepository.Add(product);

            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, [])
                .Create();

            var actionResult = await _controller.GetProduct(product.Id, request);

            actionResult.Result.ShouldNotBeNull();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            var contentResult = actionResult.Result as OkObjectResult;
            contentResult.ShouldNotBeNull();
            var response = JsonConvert.DeserializeObject<GetProductResponse>(contentResult.Value!.ToString()!);
            response.ShouldBeEquivalentTo(_extensions.ToGetProductResponse(product));
        }

        [Fact]
        public async Task GetProduct_DefaultsToWildCard_WhenOnlyFieldMaskIsNotMatched()
        {
            var product = new ProductViewTestDataBuilder().Build();
            _productRepository.Add(product);

            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, ["UnmatchedField"])
                .Create();

            var actionResult = await _controller.GetProduct(product.Id, request);

            actionResult.Result.ShouldNotBeNull();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            var contentResult = actionResult.Result as OkObjectResult;
            contentResult.ShouldNotBeNull();
            var response = JsonConvert.DeserializeObject<GetProductResponse>(contentResult.Value!.ToString()!);
            response.ShouldBeEquivalentTo(_extensions.ToGetProductResponse(product));
        }

        [Fact]
        public async Task GetProduct_ReturnsValidMasks_WhenInvalidMasksArePassed()
        {
            var product = new ProductViewTestDataBuilder().Build();
            _productRepository.Add(product);

            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, ["UnmatchedField", "Price", "Name"])
                .Create();

            var actionResult = await _controller.GetProduct(product.Id, request);

            actionResult.Result.ShouldNotBeNull();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            var contentResult = actionResult.Result as OkObjectResult;
            contentResult.ShouldNotBeNull();
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentResult.Value!.ToString()!);
            response!.Count.ShouldBeEquivalentTo(2);
            response.ShouldContainKeyAndValue("Name", product.Name);
            response.ShouldContainKeyAndValue("Price", product.Price.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public async Task GetProduct_ReturnsPartialProduct_WhenFieldMaskIsSpecified()
        {
            var product = new ProductViewTestDataBuilder().Build();
            _productRepository.Add(product);

            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, ["name"])
                .Create();

            var actionResult = await _controller.GetProduct(product.Id, request);

            actionResult.Result.ShouldNotBeNull();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            var result = actionResult.Result as OkObjectResult;
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(result!.Value!.ToString()!);
            response!.ShouldContainKey("Name");
            response!.Count.ShouldBeEquivalentTo(1);
        }

        [Fact]
        public async Task GetProduct_ReturnsPartialProduct_WhenNestedFieldMaskIsSpecified()
        {
            var product = new ProductViewTestDataBuilder().Build();
            _productRepository.Add(product);

            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, ["dimensions.width"])
                .Create();

            var actionResult = await _controller.GetProduct(product.Id, request);

            actionResult.Result.ShouldNotBeNull();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            var result = actionResult.Result as OkObjectResult;
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(result!.Value!.ToString()!);
            response!.ShouldContainKey("Dimensions");
            var dimensions = response!["Dimensions"] as JObject;
            dimensions!.ShouldContainKey("Width");
            dimensions!.Count.ShouldBeEquivalentTo(1);
        }

        [Fact]
        public async Task GetProduct_ReturnsAllFields_WhenPartialRequestIsMade()
        {
            var product = new ProductViewTestDataBuilder().Build();
            _productRepository.Add(product);

            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, ["dimensions.*"])
                .Create();

            var actionResult = await _controller.GetProduct(product.Id, request);

            actionResult.Result.ShouldNotBeNull();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            var result = actionResult.Result as OkObjectResult;
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(result!.Value!.ToString()!);
            response!.ShouldContainKey("Dimensions");
            var dimensions = response!["Dimensions"] as JObject;
            dimensions!.Count.ShouldBeEquivalentTo(3);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var request = _fixture.Create<GetProductRequest>();

            var result = await _controller.GetProduct(999, request);

            result.Result.ShouldBeOfType<NotFoundResult>();
        }
    }
}