// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.CreateSupplier;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.Controllers;

[ApiController]
[Route("supplier")]
public class CreateSupplierController(
    ICommandHandler<CreateSupplierCommand> createSupplier,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a supplier", Tags = ["Suppliers"])]
    [ProducesResponseType(typeof(CreateSupplierResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateSupplierResponse>> CreateSupplier([FromBody] CreateSupplierRequest request)
    {
        var supplier = mapper.Map<DomainModels.Supplier>(request);
        await createSupplier.Handle(new CreateSupplierCommand { Supplier = supplier });

        var response = mapper.Map<CreateSupplierResponse>(supplier);
        return Ok(response);
    }
}
