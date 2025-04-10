// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Commands.UpdateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventory;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[ApiController]
[Route("inventory")]
public class UpdateInventoryController(
    IQueryHandler<GetInventoryQuery, DomainModels.Inventory?> getInventory,
    ICommandHandler<UpdateInventoryCommand> updateInventory,
    IMapper mapper)
    : ControllerBase
{
    [HttpPatch("{id:long}")]
    [SwaggerOperation(Summary = "Update Inventory association between a Supplier and a Product", Tags = ["Inventory"])]
    [ProducesResponseType(typeof(UpdateInventoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateInventoryResponse>> UpdateInventory(
        [FromRoute] long id,
        [FromBody] UpdateInventoryRequest request)
    {
        DomainModels.Inventory? existingInventory = await getInventory.Handle(new GetInventoryQuery { Id = id });

        if (existingInventory == null)
        {
            return NotFound();
        }

        await updateInventory.Handle(new UpdateInventoryCommand { Request = request, Inventory = existingInventory });

        var updatedInventory = await getInventory.Handle(new GetInventoryQuery { Id = id });

        var response = mapper.Map<UpdateInventoryResponse>(updatedInventory);
        return Ok(response);
    }
}
