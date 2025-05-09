// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.PhoneNumber.ApplicationLayer.Commands.DeletePhoneNumber;
using backend.PhoneNumber.ApplicationLayer.Queries.GetPhoneNumber;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.PhoneNumber.Controllers;

[ApiController]
[Route("phoneNumber")]
public class DeletePhoneNumberController(
    IAsyncQueryHandler<GetPhoneNumberQuery, DomainModels.PhoneNumber?> getPhoneNumber,
    ICommandHandler<DeletePhoneNumberCommand> deletePhoneNumber)
    : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a phone number", Tags = ["PhoneNumbers"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeletePhoneNumber(
        [FromRoute] long id,
        [FromQuery] DeletePhoneNumberRequest request)
    {
        var phoneNumber = await getPhoneNumber.Handle(new GetPhoneNumberQuery { Id = id });
        if (phoneNumber == null)
        {
            return NotFound();
        }

        await deletePhoneNumber.Handle(new DeletePhoneNumberCommand { Id = id });
        return NoContent();
    }
}
