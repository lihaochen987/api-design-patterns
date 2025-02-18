// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Shared.CommandHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.InventoryControllers;

[ApiController]
[Route("inventory")]
public class CreateInventoryController(
    ICommandHandler<CreateInventoryCommand> createInventory,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Associate Inventory between a Supplier and a Product", Tags = ["Inventory"])]
    [ProducesResponseType(typeof(CreateInventoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateInventoryResponse>> CreateInventory([FromBody] CreateInventoryRequest request)
    {
        DomainModels.Inventory inventory = mapper.Map<DomainModels.Inventory>(request);
        await createInventory.Handle(new CreateInventoryCommand { Inventory = inventory });
        var response = mapper.Map<CreateInventoryResponse>(inventory);

        return CreatedAtAction(
            "GetInventory",
            "GetInventory",
            new { id = inventory.Id },
            response);
    }
}
