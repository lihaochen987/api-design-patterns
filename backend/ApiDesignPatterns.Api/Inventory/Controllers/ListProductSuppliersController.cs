// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.GetSupplierView;
using backend.Supplier.Controllers;
using backend.Supplier.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[Route("{productId:decimal}/inventory")]
[ApiController]
public class ListProductSuppliersController(
    IQueryHandler<ListInventoryQuery, PagedInventory> listInventory,
    IQueryHandler<GetSupplierViewQuery, SupplierView?> getSupplierView,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List suppliers for a given product", Tags = ["Inventory"])]
    [ProducesResponseType(typeof(ListProductSuppliersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListProductSuppliersResponse>>> ListInventory(
        [FromQuery] ListProductSuppliersRequest request, decimal productId)
    {
        var inventoryResult = await listInventory.Handle(new ListInventoryQuery
        {
            Filter = $"ProductId == {productId}", PageToken = request.PageToken, MaxPageSize = request.MaxPageSize
        });

        Task<SupplierView?>[] supplierTasks = inventoryResult.Inventory
            .Select(inventoryView => getSupplierView.Handle(new GetSupplierViewQuery { Id = inventoryView.SupplierId }))
            .ToArray();

        await Task.WhenAll(supplierTasks);

        List<SupplierView?> result = supplierTasks
            .Select(task => task.Result)
            .Where(supplier => supplier != null)
            .ToList();

        ListProductSuppliersResponse response = new()
        {
            Results = mapper.Map<List<GetSupplierResponse>>(result), NextPageToken = inventoryResult.NextPageToken
        };

        return Ok(response);
    }
}
