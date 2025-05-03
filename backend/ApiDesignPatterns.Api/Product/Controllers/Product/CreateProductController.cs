using backend.Product.ApplicationLayer.Commands.CacheCreateProductResponse;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Queries.GetCreateProductFromCache;
using backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;
using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.Product;

[ApiController]
[Route("product")]
public class CreateProductController(
    ICommandHandler<CreateProductCommand> createProduct,
    ISyncQueryHandler<MapCreateProductResponseQuery, CreateProductResponse> createProductResponse,
    ISyncQueryHandler<MapCreateProductRequestQuery, DomainModels.Product> mapProductRequest,
    IAsyncQueryHandler<GetCreateProductFromCacheQuery, GetCreateProductFromCacheResult> getCreateProductFromCache,
    ICommandHandler<CacheCreateProductResponseCommand> cacheCreateProductResponse)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a product", Tags = ["Products"])]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var cachedProduct =
            await getCreateProductFromCache.Handle(
                new GetCreateProductFromCacheQuery { RequestId = request.RequestId, CreateProductRequest = request });

        if (cachedProduct.CreateProductResponse != null)
        {
            return CreatedAtAction(
                "GetProduct",
                "GetProduct",
                new { id = cachedProduct.CreateProductResponse.Id },
                cachedProduct);
        }

        var product = mapProductRequest.Handle(new MapCreateProductRequestQuery { Request = request });
        await createProduct.Handle(new CreateProductCommand { Product = product });
        var response = createProductResponse.Handle(new MapCreateProductResponseQuery { Product = product });

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
}
