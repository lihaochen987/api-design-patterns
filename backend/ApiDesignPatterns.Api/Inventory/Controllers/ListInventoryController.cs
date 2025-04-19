// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[Route("inventory")]
[ApiController]
public class ListInventoryController(
    IQueryHandler<ListInventoryQuery, PagedInventory> listInventory,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List inventory", Tags = ["Inventory"])]
    [ProducesResponseType(typeof(ListInventoryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListInventoryResponse>>> ListInventory(
        [FromQuery] ListInventoryRequest request)
    {
        PagedInventory result =
            await listInventory.Handle(new ListInventoryQuery { Request = request });

        ListInventoryResponse response = new()
        {
            Results = mapper.Map<List<GetInventoryResponse>>(result.Inventory), NextPageToken = result.NextPageToken
        };

        return Ok(response);
    }
}
