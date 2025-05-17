// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.


using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Commands.UpdateUser;
using backend.User.ApplicationLayer.Queries.GetUser;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.User.Controllers;

[ApiController]
[Route("user")]
public class UpdateUserController(
    IAsyncQueryHandler<GetUserQuery, DomainModels.User?> getUser,
    ICommandHandler<UpdateUserCommand> updateUser,
    IMapper mapper)
    : ControllerBase
{
    [HttpPatch("{id:long}")]
    [SwaggerOperation(Summary = "Update a user", Tags = ["Users"])]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateUserResponse>> UpdateUser(
        [FromRoute] long id,
        [FromBody] UpdateUserRequest request)
    {
        var existingUser = await getUser.Handle(new GetUserQuery { Id = id });

        if (existingUser == null)
        {
            return NotFound();
        }

        await updateUser.Handle(new UpdateUserCommand { Request = request, User = existingUser });

        var updatedUser = await getUser.Handle(new GetUserQuery { Id = id });

        if (updatedUser == null)
        {
            return BadRequest();
        }

        var response = mapper.Map<UpdateUserResponse>(updatedUser);
        return Ok(response);
    }
}
