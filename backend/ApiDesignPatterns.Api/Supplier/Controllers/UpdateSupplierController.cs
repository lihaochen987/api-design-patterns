// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.


using AutoMapper;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.Controllers;

[ApiController]
[Route("supplier")]
public class UpdateSupplierController(
    IAsyncQueryHandler<GetSupplierQuery, DomainModels.Supplier?> getSupplier,
    ICommandHandler<UpdateSupplierCommand> updateSupplier,
    IMapper mapper)
    : ControllerBase
{
    [HttpPatch("{id:long}")]
    [SwaggerOperation(Summary = "Update a supplier", Tags = ["Suppliers"])]
    [ProducesResponseType(typeof(UpdateSupplierResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateSupplierResponse>> UpdateSupplier(
        [FromRoute] long id,
        [FromBody] UpdateSupplierRequest request)
    {
        var existingSupplier = await getSupplier.Handle(new GetSupplierQuery { Id = id });

        if (existingSupplier == null)
        {
            return NotFound();
        }

        await updateSupplier.Handle(new UpdateSupplierCommand { Request = request, Supplier = existingSupplier });

        var updatedSupplier = await getSupplier.Handle(new GetSupplierQuery { Id = id });

        var response = mapper.Map<UpdateSupplierResponse>(updatedSupplier);
        return Ok(response);
    }
}
