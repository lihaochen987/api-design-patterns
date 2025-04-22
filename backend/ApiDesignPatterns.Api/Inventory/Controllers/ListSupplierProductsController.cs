// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Queries.GetProductsFromInventory;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Product.ApplicationLayer.Queries.GetProductResponse;
using backend.Product.Controllers.Product;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[Route("{supplierId:decimal}/products")]
[ApiController]
public class ListSupplierProductsController(
    IAsyncQueryHandler<ListInventoryQuery, PagedInventory> listInventory,
    IAsyncQueryHandler<GetProductResponseQuery, GetProductResponse?> getProductResponse,
    ISyncQueryHandler<GetProductsFromInventoryQuery, List<GetProductResponse?>> getProductsFromInventory,
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

        var productTasks = inventoryResult.Inventory
            .Select(inventoryView =>
                getProductResponse.Handle(new GetProductResponseQuery { Id = inventoryView.ProductId }))
            .ToArray();

        var products = (await Task.WhenAll(productTasks)).ToList();

        var result =
            getProductsFromInventory.Handle(new GetProductsFromInventoryQuery { Products = products });

        ListSupplierProductsResponse response = new()
        {
            Results = mapper.Map<List<GetProductResponse>>(result), NextPageToken = inventoryResult.NextPageToken
        };

        return Ok(response);
    }
}
