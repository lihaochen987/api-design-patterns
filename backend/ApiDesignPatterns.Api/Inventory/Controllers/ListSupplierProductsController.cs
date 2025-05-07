// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Product.ApplicationLayer.Queries.BatchGetProductResponses;
using backend.Product.Controllers.Product;
using backend.Shared;
using backend.Shared.QueryHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[Route("{supplierId:decimal}/products")]
[ApiController]
public class ListSupplierProductsController(
    IAsyncQueryHandler<ListInventoryQuery, PagedInventory> listInventory,
    IAsyncQueryHandler<BatchGetProductResponsesQuery, Result<List<GetProductResponse>>> batchGetProducts,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List products for a given supplier", Tags = ["Inventory"])]
    [ProducesResponseType(typeof(ListSupplierProductsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListSupplierProductsResponse>>> ListSupplierProducts(
        [FromQuery] ListSupplierProductsRequest request, decimal supplierId)
    {
        var inventoryResult = await listInventory.Handle(new ListInventoryQuery
        {
            Filter = $"SupplierId == {supplierId}", PageToken = request.PageToken, MaxPageSize = request.MaxPageSize
        });

        var productIds = inventoryResult.Inventory.Select(x => x.ProductId).ToList();
        var products = await batchGetProducts.Handle(new BatchGetProductResponsesQuery { ProductIds = productIds });

        if (products.Value == null)
        {
            return BadRequest();
        }

        ListSupplierProductsResponse response = new()
        {
            Results = mapper.Map<List<GetProductResponse>>(products.Value), NextPageToken = inventoryResult.NextPageToken
        };

        return Ok(response);
    }
}
