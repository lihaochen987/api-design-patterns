// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Queries.GetSuppliersByIds;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Shared.QueryHandler;
using backend.Supplier.Controllers;
using backend.Supplier.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[Route("{productId:decimal}/suppliers")]
[ApiController]
public class ListProductSuppliersController(
    IAsyncQueryHandler<ListInventoryQuery, PagedInventory> listInventory,
    IAsyncQueryHandler<GetSuppliersByIdsQuery, List<SupplierView>> getSuppliersByIds,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List suppliers for a given product", Tags = ["Inventory"])]
    [ProducesResponseType(typeof(ListProductSuppliersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ListProductSuppliersResponse>> ListProductSuppliers(
        [FromQuery] ListProductSuppliersRequest request, decimal productId)
    {
        var inventoryResult = await listInventory.Handle(new ListInventoryQuery
        {
            Filter = $"ProductId == {productId}", PageToken = request.PageToken, MaxPageSize = request.MaxPageSize
        });
        var supplierIds = inventoryResult.Inventory.Select(x => x.SupplierId).ToList();
        var suppliers = await getSuppliersByIds.Handle(new GetSuppliersByIdsQuery { SupplierIds = supplierIds });

        ListProductSuppliersResponse response = new()
        {
            Results = mapper.Map<List<GetSupplierResponse>>(suppliers),
            NextPageToken = inventoryResult.NextPageToken
        };

        return Ok(response);
    }
}
