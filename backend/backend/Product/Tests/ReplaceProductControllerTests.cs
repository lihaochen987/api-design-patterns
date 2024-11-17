// using backend.Database;
// using backend.Product.Database;
// using backend.Product.ProductControllers;
// using backend.Product.Tests.Builders;
// using backend.Shared;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Shouldly;
// using Xunit;
//
// namespace backend.Product.Tests;
//
// [Collection("SequentialExecutionCollection")]
// public class ReplaceProductControllerTests : IDisposable
// {
//     private readonly ReplaceProductController _controller;
//     private readonly ProductDbContext _dbContext;
//     private readonly ReplaceProductExtensions _extensions;
//
//     public ReplaceProductControllerTests()
//     {
//         var options = new DbContextOptionsBuilder<ProductDbContext>()
//             .UseNpgsql("Host=localhost;Database=mytestdatabase;Username=myusername;Password=mypassword")
//             .Options;
//
//         var db = new ProductDbContext(options);
//         db.Database.EnsureCreated();
//         _dbContext = db;
//         _extensions = new ReplaceProductExtensions(new TypeParser());
//         _controller = new ReplaceProductController(_dbContext, _extensions);
//     }
//
//     [Fact]
//     public async Task ReplaceProduct_Should_ReturnOk_WithUpdatedProduct_When_ProductExists()
//     {
//         var originalProduct = new ProductTestDataBuilder().Build();
//         _dbContext.Products.Add(originalProduct);
//         await _dbContext.SaveChangesAsync();
//
//         var request = _extensions.ToReplaceProductRequest(originalProduct);
//
//         var result = await _controller.ReplaceProduct(originalProduct.Id, request);
//
//         result.Result.ShouldBeOfType<OkObjectResult>();
//         var response = result.Result as OkObjectResult;
//         response!.Value.ShouldBeOfType<ReplaceProductResponse>();
//         var replaceProductResponse = response.Value as ReplaceProductResponse;
//         replaceProductResponse.ShouldBeEquivalentTo(_extensions.ToReplaceProductResponse(originalProduct));
//         var updatedProduct = await _dbContext.Products.FindAsync(originalProduct.Id);
//         updatedProduct.ShouldNotBeNull();
//         updatedProduct.ShouldBeEquivalentTo(originalProduct);
//     }
//
//     public void Dispose()
//     {
//         _dbContext.Database.EnsureDeleted();
//         _dbContext.Dispose();
//         GC.SuppressFinalize(this);
//     }
// }