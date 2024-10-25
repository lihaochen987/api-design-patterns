using System.Globalization;
using AutoFixture;
using backend.Database;
using backend.Product.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace backend.Product.Tests
{
    public class GetProductControllerTests: IDisposable
    {
        private readonly Fixture _fixture = new();
        private readonly GetProductController _controller;
        private readonly ApplicationDbContext _dbContext;

        public GetProductControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("app.db")
                .Options;

            var db = new ApplicationDbContext(options);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            _dbContext = db;
            _controller = new GetProductController(_dbContext);
        }

        [Fact]
        public async Task GetProduct_ReturnsFullProduct_WhenFieldMaskIsWildcard()
        {
            var product = _fixture.Create<Product>();
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
            response.ShouldNotBeNull();
            response.Name.ShouldBeEquivalentTo(product.Name);
            response.Price.ShouldBeEquivalentTo(product.Price.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public async Task GetProduct_ReturnsPartialProduct_WhenFieldMaskIsSpecified()
        {
            var product = _fixture.Create<Product>();
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
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var request = _fixture.Build<GetProductRequest>()
                .With(r => r.FieldMask, ["*"])
                .Create();

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