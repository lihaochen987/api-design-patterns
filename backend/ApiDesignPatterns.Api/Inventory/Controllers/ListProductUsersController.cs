// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.GetUsersByIds;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Shared.QueryHandler;
using backend.User.Controllers;
using backend.User.DomainModels;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[Route("{productId:decimal}/users")]
[ApiController]
public class ListProductUsersController(
    IAsyncQueryHandler<ListInventoryQuery, PagedInventory> listInventory,
    IAsyncQueryHandler<GetUsersByIdsQuery, List<UserView>> getUsersByIds,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List users for a given product", Tags = ["Inventory"])]
    [ProducesResponseType(typeof(ListProductUsersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ListProductUsersResponse>> ListProductUsers(
        [FromQuery] ListProductUsersRequest request, decimal productId)
    {
        var inventoryResult = await listInventory.Handle(new ListInventoryQuery
        {
            Filter = $"ProductId == {productId}", PageToken = request.PageToken, MaxPageSize = request.MaxPageSize
        });
        var userIds = inventoryResult.Inventory.Select(x => x.UserId).ToList();
        var users = await getUsersByIds.Handle(new GetUsersByIdsQuery { UserIds = userIds });

        ListProductUsersResponse response = new()
        {
            Results = mapper.Map<List<GetUserResponse>>(users),
            NextPageToken = inventoryResult.NextPageToken
        };

        return Ok(response);
    }
}
