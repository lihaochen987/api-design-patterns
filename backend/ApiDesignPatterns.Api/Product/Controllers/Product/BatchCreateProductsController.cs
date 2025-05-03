using backend.Product.ApplicationLayer.Commands.BatchCreateProducts;
using backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;
using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.Product;

[ApiController]
public class BatchCreateProductsController(
    ICommandHandler<BatchCreateProductsCommand> batchCreateProducts,
    ISyncQueryHandler<MapCreateProductRequestQuery, DomainModels.Product> mapCreateProductRequest,
    ISyncQueryHandler<MapCreateProductResponseQuery, CreateProductResponse> mapCreateProductResponse)
    : ControllerBase
{
    [HttpPost("product:batchCreate")]
    [SwaggerOperation(Summary = "Create a batch of products", Tags = ["Products"])]
    [ProducesResponseType(typeof(BatchCreateProductsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BatchCreateProductsResponse>> GetBatchProducts(
        [FromBody] BatchCreateProductsRequest request)
    {
        if (request.Products == null || !request.Products.Any())
        {
            return BadRequest("No products provided for batch creation");
        }

        var products = request.Products
            .Select(createProductRequest =>
                mapCreateProductRequest.Handle(new MapCreateProductRequestQuery { Request = createProductRequest }))
            .ToList();

        await batchCreateProducts.Handle(new BatchCreateProductsCommand { Products = products });

        var productResponses = products
            .Select(product =>
                mapCreateProductResponse.Handle(new MapCreateProductResponseQuery { Product = product }))
            .ToList();

        var response = new BatchCreateProductsResponse { Results = productResponses };

        return CreatedAtAction(
            actionName: nameof(BatchGetProductsController.GetBatchProducts),
            controllerName: "BatchGetProducts",
            routeValues: new { Ids = products.Select(p => p.Id).ToList() },
            value: response);
    }
}
