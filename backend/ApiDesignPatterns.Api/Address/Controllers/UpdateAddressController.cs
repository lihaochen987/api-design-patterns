// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.


using backend.Address.ApplicationLayer.Commands.UpdateAddress;
using backend.Address.ApplicationLayer.Queries.GetAddress;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Address.Controllers;

[ApiController]
[Route("address")]
public class UpdateAddressController(
    IAsyncQueryHandler<GetAddressQuery, DomainModels.Address?> getAddress,
    ICommandHandler<UpdateAddressCommand> updateAddress,
    IMapper mapper)
    : ControllerBase
{
    [HttpPatch("{id:long}")]
    [SwaggerOperation(Summary = "Update an address", Tags = ["Addresses"])]
    [ProducesResponseType(typeof(UpdateAddressResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateAddressResponse>> UpdateAddress(
        [FromRoute] long id,
        [FromBody] UpdateAddressRequest request)
    {
        var existingAddress = await getAddress.Handle(new GetAddressQuery { Id = id });

        if (existingAddress == null)
        {
            return NotFound();
        }

        await updateAddress.Handle(new UpdateAddressCommand { Request = request, Address = existingAddress });

        var updatedAddress = await getAddress.Handle(new GetAddressQuery { Id = id });

        if (updatedAddress == null)
        {
            return BadRequest();
        }

        var response = mapper.Map<UpdateAddressResponse>(updatedAddress);
        return Ok(response);
    }
}
