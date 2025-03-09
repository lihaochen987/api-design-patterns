// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Commands.DeleteSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.SupplierControllers;

[ApiController]
[Route("supplier")]
public class DeleteSupplierController(
    IQueryHandler<GetSupplierQuery, DomainModels.Supplier?> getSupplier,
    ICommandHandler<DeleteSupplierCommand> deleteSupplier)
    : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a supplier", Tags = ["Suppliers"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteSupplier(
        [FromRoute] long id,
        [FromQuery] DeleteSupplierRequest request)
    {
        var supplier = await getSupplier.Handle(new GetSupplierQuery { Id = id });
        if (supplier == null)
        {
            return NotFound();
        }

        await deleteSupplier.Handle(new DeleteSupplierCommand { Id = id });
        return NoContent();
    }
}
