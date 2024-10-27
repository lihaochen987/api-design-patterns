using System.Globalization;
using AutoFixture;
using backend.Database;
using backend.Product.Controllers;
using backend.Shared.FieldMasks;
using backend.Shared.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;
using FieldMaskPatternCleaner = backend.Shared.FieldMasks.FieldMaskPatternCleaner;

namespace backend.Product.Tests
{
    [Collection("SequentialExecutionCollection")]
    public class GetProductControllerTests : IDisposable
    {
        private readonly Fixture _fixture = new();
        private readonly GetProductController _controller;
        private readonly ApplicationDbContext _dbContext;

        public GetProductControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite($"Filename=:memory:{Guid.NewGuid()};Mode=Memory;Cache=Shared")
                .Options;

            var db = new ApplicationDbContext(options);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            _dbContext = db;
            IFieldMaskSerializer fieldMaskSerializer = new FieldMaskSerializer(
                new FieldMaskSelector(new ReflectionUtility(), new FieldMaskPathBuilder()),
                new FieldMaskPathBuilder(), new FieldMaskPatternCleaner());

            _controller = new GetProductController(_dbContext, fieldMaskSerializer);
        }


        [Fact]
        public async Task GetProduct_ReturnsFullProduct_WhenFieldMaskIsWildcard()
        {
            var product = _fixture.Create<DomainModels.Product>();
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, ["*"])
                .Create();

            var actionResult = await _controller.GetProduct(product.Id, request);

            actionResult.Result.ShouldNotBeNull();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            var contentResult = actionResult.Result as OkObjectResult;
            contentResult.ShouldNotBeNull();
            var response = JsonConvert.DeserializeObject<GetProductResponse>(contentResult.Value!.ToString()!);
            response.ShouldBeEquivalentTo(product.ToGetProductResponse());
        }

        [Fact]
        public async Task GetProduct_ReturnsFullProduct_WhenFieldMaskIsEmpty()
        {
            var product = _fixture.Create<DomainModels.Product>();
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, [])
                .Create();

            var actionResult = await _controller.GetProduct(product.Id, request);

            actionResult.Result.ShouldNotBeNull();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            var contentResult = actionResult.Result as OkObjectResult;
            contentResult.ShouldNotBeNull();
            var response = JsonConvert.DeserializeObject<GetProductResponse>(contentResult.Value!.ToString()!);
            response.ShouldBeEquivalentTo(product.ToGetProductResponse());
        }

        [Fact]
        public async Task GetProduct_DefaultsToWildCard_WhenOnlyFieldMaskIsNotMatched()
        {
            var product = _fixture.Create<DomainModels.Product>();
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, ["UnmatchedField"])
                .Create();

            var actionResult = await _controller.GetProduct(product.Id, request);

            actionResult.Result.ShouldNotBeNull();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            var contentResult = actionResult.Result as OkObjectResult;
            contentResult.ShouldNotBeNull();
            var response = JsonConvert.DeserializeObject<GetProductResponse>(contentResult.Value!.ToString()!);
            response.ShouldBeEquivalentTo(product.ToGetProductResponse());
        }

        [Fact]
        public async Task GetProduct_ReturnsValidMasks_WhenInvalidMasksArePassed()
        {
            var product = _fixture.Create<DomainModels.Product>();
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

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
            var product = _fixture.Create<DomainModels.Product>();
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

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
            var product = _fixture.Create<DomainModels.Product>();
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

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
            var product = _fixture.Create<DomainModels.Product>();
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

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

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}