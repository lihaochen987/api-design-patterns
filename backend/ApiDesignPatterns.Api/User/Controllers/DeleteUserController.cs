// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Commands.DeleteUser;
using backend.User.ApplicationLayer.Queries.GetUser;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.User.Controllers;

[ApiController]
[Route("user")]
public class DeleteUserController(
    IAsyncQueryHandler<GetUserQuery, DomainModels.User?> getUser,
    ICommandHandler<DeleteUserCommand> deleteUser)
    : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a user", Tags = ["Users"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteUser(
        [FromRoute] long id,
        [FromQuery] DeleteUserRequest request)
    {
        var user = await getUser.Handle(new GetUserQuery { Id = id });
        if (user == null)
        {
            return NotFound();
        }

        await deleteUser.Handle(new DeleteUserCommand { Id = id });
        return NoContent();
    }
}
