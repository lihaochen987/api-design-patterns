// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.


using AutoMapper;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.SupplierControllers;

[ApiController]
[Route("supplier")]
public class UpdateSupplierController(
    ISupplierApplicationService applicationService,
    IQueryHandler<GetSupplierQuery, DomainModels.Supplier> getSupplier,
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

        await applicationService.UpdateSupplierAsync(request, existingSupplier, id);

        var response = mapper.Map<UpdateSupplierResponse>(existingSupplier);
        return Ok(response);
    }
}
