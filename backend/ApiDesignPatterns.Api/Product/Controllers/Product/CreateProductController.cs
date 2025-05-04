using backend.Product.ApplicationLayer.Commands.CacheCreateProductResponse;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Queries.GetCreateProductFromCache;
using backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;
using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Shared.CommandHandler;
using backend.Shared.QueryProcessor;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.Product;

[ApiController]
[Route("product")]
public class CreateProductController(
    IQueryProcessor queries,
    ICommandHandler<CreateProductCommand> createProduct,
    ICommandHandler<CacheCreateProductResponseCommand> cacheCreateProductResponse)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a product", Tags = ["Products"])]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
    {
        string hash = GenerateHash(request);

        var getCreateProductFromCacheQuery =
            new GetCreateProductFromCacheQuery { RequestId = request.RequestId, CreateProductRequest = request };
        var cachedProduct = await queries.Process(getCreateProductFromCacheQuery);

        if (cachedProduct.CreateProductResponse == null)
        {
            var mapCreateProductRequestQuery = new MapCreateProductRequestQuery { Request = request };
            var product = queries.Process(mapCreateProductRequestQuery).Result;
            await createProduct.Handle(new CreateProductCommand { Product = product });

            var mapCreateProductResponseQuery = new MapCreateProductResponseQuery { Product = product };
            var response = queries.Process(mapCreateProductResponseQuery).Result;

            if (request.RequestId != null)
            {
                await cacheCreateProductResponse.Handle(new CacheCreateProductResponseCommand
                {
                    RequestId = request.RequestId, CreateProductRequest = request, CreateProductResponse = response
                });
            }

            return CreatedAtAction(
                "GetProduct",
                "GetProduct",
                new { id = product.Id },
                response);
        }

        if (hash == cachedProduct.Hash)
        {
            return CreatedAtAction(
                "GetProduct",
                "GetProduct",
                new { id = cachedProduct.CreateProductResponse.Id },
                cachedProduct);
        }

        return Conflict();
    }

    private static string GenerateHash<T>(T obj)
    {
        string json = System.Text.Json.JsonSerializer.Serialize(obj);
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(json));

        return Convert.ToBase64String(hashBytes);
    }
}
