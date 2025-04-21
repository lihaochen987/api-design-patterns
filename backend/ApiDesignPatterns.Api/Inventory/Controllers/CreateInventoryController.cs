// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndSupplier;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[ApiController]
[Route("inventory")]
public class CreateInventoryController(
    ICommandHandler<CreateInventoryCommand> createInventory,
    IAsyncQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?> getInventoryByProductAndSupplier,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Associate Inventory between a Supplier and a Product", Tags = ["Inventory"])]
    [ProducesResponseType(typeof(CreateInventoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateInventoryResponse>> CreateInventory([FromBody] CreateInventoryRequest request)
    {
        var existingInventory = await getInventoryByProductAndSupplier.Handle(new GetInventoryByProductAndSupplierQuery
        {
            ProductId = request.ProductId, SupplierId = request.SupplierId
        });

        if (existingInventory != null)
        {
            return Conflict();
        }

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
