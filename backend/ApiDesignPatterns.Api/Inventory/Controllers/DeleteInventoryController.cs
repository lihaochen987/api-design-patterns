// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Commands.DeleteInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[ApiController]
[Route("inventory")]
public class DeleteInventoryController(
    IAsyncQueryHandler<GetInventoryByIdQuery, DomainModels.Inventory?> getInventoryById,
    ICommandHandler<DeleteInventoryCommand> deleteInventory)
    : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a inventory", Tags = ["Inventory"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteInventory(
        [FromRoute] long id,
        [FromQuery] DeleteInventoryRequest request)
    {
        DomainModels.Inventory? inventory = await getInventoryById.Handle(new GetInventoryByIdQuery { Id = id });
        if (inventory == null)
        {
            return NotFound();
        }

        await deleteInventory.Handle(new DeleteInventoryCommand { Id = id });
        return NoContent();
    }
}
