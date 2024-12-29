// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.


using AutoMapper;
using backend.Supplier.ApplicationLayer;
using backend.Supplier.DomainModels;
using backend.Supplier.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.SupplierControllers;

[ApiController]
[Route("supplier")]
public class UpdateSupplierController(
    ISupplierApplicationService applicationService,
    SupplierFieldMaskConfiguration maskConfiguration,
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
        var supplier = await applicationService.GetSupplierAsync(id);

        if (supplier == null)
        {
            return NotFound();
        }

        (string firstName, string lastName, string email, Address address, PhoneNumber phoneNumber) =
            maskConfiguration.GetUpdatedSupplierValues(request, supplier);
        supplier.FirstName = firstName;
        supplier.LastName = lastName;
        supplier.Email = email;
        supplier.Address = address;
        supplier.PhoneNumber = phoneNumber;
        await applicationService.UpdateSupplierAsync(supplier, id);

        var response = mapper.Map<UpdateSupplierResponse>(supplier);
        return Ok(response);
    }
}
