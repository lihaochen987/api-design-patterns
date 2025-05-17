using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Queries.ListUsers;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.User.Controllers;

[Route("users")]
[ApiController]
public class ListUsersController(
    IAsyncQueryHandler<ListUsersQuery, PagedUsers> listUsers,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List users", Tags = ["Users"])]
    [ProducesResponseType(typeof(ListUsersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListUsersResponse>>> ListUsers(
        [FromQuery] ListUsersRequest request)
    {
        PagedUsers result = await listUsers.Handle(new ListUsersQuery { Request = request });

        ListUsersResponse response = new()
        {
            Results = mapper.Map<List<GetUserResponse>>(result.Users), NextPageToken = result.NextPageToken
        };

        return Ok(response);
    }
}
