// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.User.ApplicationLayer.Commands.CreateUser;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.User.Controllers;

[ApiController]
[Route("user")]
public class CreateUserController(
    ICommandHandler<CreateUserCommand> createUser,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a user", Tags = ["Users"])]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateUserResponse>> CreateUser([FromBody] CreateUserRequest request)
    {
        var user = mapper.Map<DomainModels.User>(request);
        await createUser.Handle(new CreateUserCommand { User = user });

        var response = mapper.Map<CreateUserResponse>(user);
        return Ok(response);
    }
}
