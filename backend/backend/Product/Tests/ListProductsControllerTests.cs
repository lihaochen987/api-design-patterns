// using backend.Product.Database;
// using backend.Product.DomainModels;
// using backend.Product.ProductControllers;
// using backend.Product.Tests.Builders;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Shouldly;
// using Xunit;
//
// namespace backend.Product.Tests;
//
// [Collection("SequentialExecutionCollection")]
// public class ListProductsControllerTests : IDisposable
// {
//     private readonly ListProductsController _controller;
//     private readonly ProductDbContext _dbContext;
//     private const int DefaultMaxPageSize = 10;
//
//     public ListProductsControllerTests()
//     {
//         var options = new DbContextOptionsBuilder<ProductDbContext>()
//             .UseNpgsql("Host=localhost;Database=mytestdatabase;Username=myusername;Password=mypassword")
//             .Options;
//
//         var db = new ProductDbContext(options);
//         db.Database.EnsureCreated();
//         _dbContext = db;
//         var extensions = new GetProductExtensions();
//         _controller = new ListProductsController(_dbContext, extensions);
//     }
//
//     [Fact]
//     public async Task ListProducts_ShouldReturnAllProducts_WhenNoPageTokenProvided()
//     {
//         var products = new ProductTestDataBuilder().CreateMany(4);
//         _dbContext.Products.AddRange(products);
//         await _dbContext.SaveChangesAsync();
//
//         var request = new ListProductsRequest { MaxPageSize = 4 };
//
//         var result = await _controller.ListProducts(request);
//
//         result.Result.ShouldNotBeNull();
//         result.Result.ShouldBeOfType<OkObjectResult>();
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse!.Results.Count().ShouldBe(4);
//     }
//
//     [Fact]
//     public async Task ListProducts_ShouldReturnProductsAfterPageToken_WhenPageTokenProvided()
//     {
//         var products = new ProductTestDataBuilder().CreateMany(4);
//         _dbContext.Products.AddRange(products);
//         await _dbContext.SaveChangesAsync();
//
//         var request = new ListProductsRequest { PageToken = "2", MaxPageSize = 2 };
//
//         var result = await _controller.ListProducts(request);
//
//         result.Result.ShouldNotBeNull();
//         result.Result.ShouldBeOfType<OkObjectResult>();
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse!.Results.Count().ShouldBe(2);
//         listProductsResponse.Results.First()!.Id.ShouldBe("3");
//         listProductsResponse.Results.Last()!.Id.ShouldBe("4");
//         listProductsResponse.NextPageToken.ShouldBeNull();
//     }
//
//     [Fact]
//     public async Task ListProducts_ShouldReturnNextPageToken_WhenMoreProductsExist()
//     {
//         var products = new ProductTestDataBuilder().CreateMany(20);
//         _dbContext.Products.AddRange(products);
//         await _dbContext.SaveChangesAsync();
//
//         var request = new ListProductsRequest { MaxPageSize = 2 };
//
//         var result = await _controller.ListProducts(request);
//
//         result.Result.ShouldNotBeNull();
//         result.Result.ShouldBeOfType<OkObjectResult>();
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse!.Results.Count().ShouldBe(2);
//         listProductsResponse.NextPageToken.ShouldBeEquivalentTo("2");
//     }
//
//     [Fact]
//     public async Task ListProducts_ShouldUseDefaults_WhenPageTokenAndMaxPageSizeNotProvided()
//     {
//         var products = new ProductTestDataBuilder().CreateMany(20);
//         _dbContext.Products.AddRange(products);
//         await _dbContext.SaveChangesAsync();
//
//         var request = new ListProductsRequest();
//
//         var result = await _controller.ListProducts(request);
//
//         result.Result.ShouldNotBeNull();
//         result.Result.ShouldBeOfType<OkObjectResult>();
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse!.Results.Count().ShouldBe(DefaultMaxPageSize);
//         listProductsResponse.NextPageToken.ShouldBeEquivalentTo(DefaultMaxPageSize.ToString());
//     }
//
//     [Fact]
//     public async Task ListProducts_ShouldReturnEmptyList_WhenNoProductsExist()
//     {
//         var request = new ListProductsRequest { MaxPageSize = 2 };
//
//         var result = await _controller.ListProducts(request);
//
//         result.Result.ShouldNotBeNull();
//         result.Result.ShouldBeOfType<OkObjectResult>();
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse!.Results.ShouldBeEmpty();
//         listProductsResponse.NextPageToken.ShouldBeNull();
//     }
//
//     [Fact]
//     public async Task ListProducts_ShouldReturnCategoryAsString()
//     {
//         var products = new ProductTestDataBuilder().CreateMany(20);
//         _dbContext.Products.AddRange(products);
//         await _dbContext.SaveChangesAsync();
//
//         var request = new ListProductsRequest();
//
//         var result = await _controller.ListProducts(request);
//
//         result.Result.ShouldNotBeNull();
//         result.Result.ShouldBeOfType<OkObjectResult>();
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse!.Results.First()!.Category.ShouldBeOfType(typeof(string));
//         listProductsResponse.Results.Count().ShouldBe(DefaultMaxPageSize);
//         listProductsResponse.NextPageToken.ShouldBeEquivalentTo(DefaultMaxPageSize.ToString());
//     }
//
//     [Fact]
//     public async Task ListProducts_WithCalculatedFieldFilter_ReturnsFilteredResults()
//     {
//         var product = new ProductTestDataBuilder().WithPriceLessThan(20).Build();
//         _dbContext.Products.Add(product);
//         await _dbContext.SaveChangesAsync();
//
//         var request = new ListProductsRequest
//         {
//             Filter = "Category == \"PetFood\" && Price < 20",
//             MaxPageSize = 10
//         };
//
//         var result = await _controller.ListProducts(request);
//
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse.ShouldNotBeNull();
//         listProductsResponse.Results.ShouldHaveSingleItem()
//             .ShouldSatisfyAllConditions(
//                 p => p!.Category.ShouldBe(product.Category.ToString()),
//                 p => decimal.Parse(p!.Price).ShouldBeLessThan(20));
//     }
//
//     [Fact]
//     public async Task ListProducts_WithNestedFieldFilter_ReturnsFilteredResults()
//     {
//         var product = new ProductTestDataBuilder().WithDimensions(new Dimensions(5, 1, 30)).Build();
//         _dbContext.Products.Add(product);
//         await _dbContext.SaveChangesAsync();
//
//         var request = new ListProductsRequest
//         {
//             Filter = "Dimensions.Length == 5 && Dimensions.Width < 20",
//             MaxPageSize = 10
//         };
//
//         var result = await _controller.ListProducts(request);
//
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse.ShouldNotBeNull();
//         listProductsResponse.Results.ShouldHaveSingleItem()
//             .ShouldSatisfyAllConditions(
//                 p => int.Parse(p!.Dimensions.Length).ShouldBeLessThan(5),
//                 p => int.Parse(p!.Dimensions.Width).ShouldBeLessThan(20));
//     }
//
//     [Fact]
//     public async Task ListProducts_WithSpacedFieldFilter_ReturnsFilteredResults()
//     {
//         var product = new ProductTestDataBuilder().WithName("Chew Toy").Build();
//         _dbContext.Products.Add(product);
//         await _dbContext.SaveChangesAsync();
//
//         var request = new ListProductsRequest
//         {
//             Filter = "Name == \"Chew Toy\"",
//             MaxPageSize = 10
//         };
//
//         var result = await _controller.ListProducts(request);
//
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse.ShouldNotBeNull();
//         listProductsResponse.Results.ShouldHaveSingleItem()
//             .ShouldSatisfyAllConditions(
//                 p => p!.Name.ShouldBe(product.Name));
//     }
//
//     [Fact]
//     public async Task ListProducts_WithFilterAndPagination_ReturnsCorrectResults()
//     {
//         var product = new ProductTestDataBuilder().WithId(2).WithCategory(Category.Beds).Build();
//         _dbContext.Products.Add(product);
//         await _dbContext.SaveChangesAsync();
//
//         var request = new ListProductsRequest
//         {
//             Filter = "Category == \"Beds\"",
//             MaxPageSize = 2,
//             PageToken = "1"
//         };
//
//         var result = await _controller.ListProducts(request);
//
//         result.Result.ShouldNotBeNull();
//         result.Result.ShouldBeOfType<OkObjectResult>();
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse!.Results.Count().ShouldBe(1);
//         listProductsResponse.Results
//             .All(p => p!.Category == product.Category.ToString())
//             .ShouldBeTrue();
//     }
//
//     [Fact]
//     public async Task ListProducts_WithFilterAndPagination_ReturnsCorrectResultsWhenScattered()
//     {
//         var firstFilteredProduct = new ProductTestDataBuilder().WithId(1).WithCategory(Category.Beds).Build();
//         var products = new ProductTestDataBuilder().WithCategory(Category.Clothing).CreateMany(10, 2);
//         var secondFilteredProducts = new ProductTestDataBuilder().WithId(12).WithCategory(Category.Beds).Build();
//         _dbContext.Products.AddRange(products);
//         _dbContext.Products.AddRange(firstFilteredProduct, secondFilteredProducts);
//         await _dbContext.SaveChangesAsync();
//
//         var request = new ListProductsRequest
//         {
//             Filter = "Category == \"Beds\"",
//             MaxPageSize = 5,
//             PageToken = "1"
//         };
//
//         var result = await _controller.ListProducts(request);
//
//         result.Result.ShouldNotBeNull();
//         result.Result.ShouldBeOfType<OkObjectResult>();
//         var response = result.Result as OkObjectResult;
//         response.ShouldNotBeNull();
//         var listProductsResponse = response.Value as ListProductsResponse;
//         listProductsResponse!.Results.Count().ShouldBe(2);
//     }
//
//     [Fact]
//     public async Task ListProducts_WithInvalidFilter_ReturnsBadRequest()
//     {
//         var request = new ListProductsRequest
//         {
//             Filter = "InvalidFilter == \"NonExistent\"",
//             MaxPageSize = 10
//         };
//
//         await _controller.ListProducts(request).ShouldThrowAsync<ArgumentException>();
//     }
//
//     public void Dispose()
//     {
//         _dbContext.Database.EnsureDeleted();
//         _dbContext.Dispose();
//         GC.SuppressFinalize(this);
//     }
// }



