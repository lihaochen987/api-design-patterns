using backend.Product.ApplicationLayer.Commands.BatchCreateProducts;
using backend.Product.ApplicationLayer.Commands.CacheCreateProductResponses;
using backend.Product.Services.Mappers;
using backend.Shared.CommandHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.Product;

[ApiController]
public class BatchCreateProductsController(
    ICommandHandler<BatchCreateProductsCommand> batchCreateProducts,
    ICommandHandler<CacheCreateProductResponsesCommand> cacheCreateProductResponses,
    IProductTypeMapper productTypeMapper)
    : ControllerBase
{
    [HttpPost("product:batchCreate")]
    [SwaggerOperation(Summary = "Create a batch of products", Tags = ["Products"])]
    [ProducesResponseType(typeof(BatchCreateProductsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BatchCreateProductsResponse>> GetBatchProducts(
        [FromBody] BatchCreateProductsRequest request)
    {
        if (!request.Products.Any())
        {
            return BadRequest("No products provided for batch creation");
        }

        var products = request.Products
            .Select(productTypeMapper.MapFromRequest)
            .ToList();

        await batchCreateProducts.Handle(new BatchCreateProductsCommand { Products = products });

        var productResponses = products
            .Select(product =>
                productTypeMapper.MapToResponse<CreateProductResponse>(product, ProductControllerMethod.Create))
            .ToList();

        if (request.RequestId != null)
        {
            await cacheCreateProductResponses.Handle(new CacheCreateProductResponsesCommand
            {
                RequestId = request.RequestId,
                CreateProductRequests = request.Products,
                CreateProductResponses = productResponses
            });
        }

        var response = new BatchCreateProductsResponse { Results = productResponses };

        return CreatedAtAction(
            actionName: nameof(BatchGetProductsController.GetBatchProducts),
            controllerName: "BatchGetProducts",
            routeValues: new { Ids = products.Select(p => p.Id).ToList() },
            value: response);
    }
}
