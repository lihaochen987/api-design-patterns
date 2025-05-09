// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Commands.DeleteAddress;
using backend.Address.ApplicationLayer.Queries.GetAddress;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Address.Controllers;

[ApiController]
[Route("address")]
public class DeleteAddressController(
    IAsyncQueryHandler<GetAddressQuery, DomainModels.Address?> getAddress,
    ICommandHandler<DeleteAddressCommand> deleteAddress)
    : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete an address", Tags = ["Addresses"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAddress(
        [FromRoute] long id,
        [FromQuery] DeleteAddressRequest request)
    {
        var address = await getAddress.Handle(new GetAddressQuery { Id = id });
        if (address == null)
        {
            return NotFound();
        }

        await deleteAddress.Handle(new DeleteAddressCommand { Id = id });
        return NoContent();
    }
}
