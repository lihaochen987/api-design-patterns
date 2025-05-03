// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.BatchGetProducts;
using backend.Shared;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.Product;

[ApiController]
public class BatchGetProductsController(
    IAsyncQueryHandler<BatchGetProductsQuery, Result<List<GetProductResponse>>> batchGetProducts)
    : ControllerBase
{
    [HttpGet("product:batchGet")]
    [SwaggerOperation(Summary = "Get a batch of products", Tags = ["Products"])]
    [ProducesResponseType(typeof(BatchGetProductsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BatchGetProductsResponse>> GetBatchProducts(
        [FromBody] BatchGetProductsRequest request)
    {
        var products = await batchGetProducts.Handle(new BatchGetProductsQuery { ProductIds = request.Ids });

        if (!products.IsSuccess || products.Value == null)
        {
            return BadRequest(products.Error);
        }

        var response = new BatchGetProductsResponse { Results = products.Value };
        return Ok(response);
    }
}
