// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Queries.GetProductsByIds;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Views;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[Route("{supplierId:decimal}/products")]
[ApiController]
public class ListSupplierProductsController(
    IAsyncQueryHandler<ListInventoryQuery, PagedInventory> listInventory,
    IAsyncQueryHandler<GetProductsByIdsQuery, List<ProductView>> getProductsByIds,
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
        var products = await getProductsByIds.Handle(new GetProductsByIdsQuery { ProductIds = productIds });

        ListSupplierProductsResponse response = new()
        {
            Results = mapper.Map<List<GetProductResponse>>(products), NextPageToken = inventoryResult.NextPageToken
        };

        return Ok(response);
    }
}
