// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.BatchDeleteProducts;
using backend.Product.ApplicationLayer.Queries.BatchGetProducts;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.Product;

[ApiController]
[Route("product")]
public class BatchDeleteProductsController(
    ICommandHandler<BatchDeleteProductsCommand> batchDeleteProducts,
    IAsyncQueryHandler<BatchGetProductsQuery, Result<List<GetProductResponse>>> batchGetProducts)
    : ControllerBase
{
    [HttpDelete(":batchDelete")]
    [SwaggerOperation(Summary = "Delete a batch of products based on id", Tags = ["Products"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> GetBatchProducts(
        [FromBody] BatchDeleteProductsRequest request)
    {
        var products = await batchGetProducts.Handle(new BatchGetProductsQuery { ProductIds = request.Ids });

        if (!products.IsSuccess || products.Value == null)
        {
            return BadRequest(products.Error);
        }

        await batchDeleteProducts.Handle(new BatchDeleteProductsCommand { ProductIds = request.Ids });
        var response = new BatchGetProductsResponse { Results = products.Value };
        return Ok(response);
    }
}
