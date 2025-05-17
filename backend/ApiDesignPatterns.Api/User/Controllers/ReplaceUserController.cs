using backend.Product.Controllers.Product;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Commands.ReplaceUser;
using backend.User.ApplicationLayer.Queries.GetUser;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.User.Controllers;

[ApiController]
[Route("user")]
public class ReplaceUserController(
    IAsyncQueryHandler<GetUserQuery, DomainModels.User?> getUser,
    ICommandHandler<ReplaceUserCommand> replaceUser,
    IMapper mapper)
    : ControllerBase
{
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Replace a user", Tags = ["Users"])]
    [ProducesResponseType(typeof(ReplaceUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReplaceProductResponse>> ReplaceUser(
        [FromRoute] long id,
        [FromBody] ReplaceUserRequest request)
    {
        var existingUser = await getUser.Handle(new GetUserQuery { Id = id });
        if (existingUser == null)
        {
            return NotFound();
        }

        var mappedUser = mapper.Map<DomainModels.User>(request);

        await replaceUser.Handle(new ReplaceUserCommand { User = mappedUser, UserId = id });

        var replacedUser = await getUser.Handle(new GetUserQuery { Id = id });
        if (replacedUser == null)
        {
            return BadRequest();
        }

        var response = mapper.Map<ReplaceUserResponse>(replacedUser);
        return Ok(response);
    }
}
