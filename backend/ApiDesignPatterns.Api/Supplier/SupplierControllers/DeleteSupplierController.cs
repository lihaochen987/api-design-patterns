// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.ApplicationLayer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.SupplierControllers;

[ApiController]
[Route("supplier")]
public class DeleteSupplierController(ISupplierApplicationService applicationService) : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a supplier", Tags = ["Suppliers"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteSupplier(
        [FromRoute] long id,
        [FromQuery] DeleteSupplierRequest request)
    {
        var supplier = await applicationService.GetSupplierAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }

        await applicationService.DeleteSupplierAsync(supplier);
        return NoContent();
    }
}
