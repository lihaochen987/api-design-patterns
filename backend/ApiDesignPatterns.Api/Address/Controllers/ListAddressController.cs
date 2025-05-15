// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Queries.ListAddress;
using backend.Shared.QueryHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Address.Controllers;

[Route("address")]
[ApiController]
public class ListAddressController(
    IAsyncQueryHandler<ListAddressQuery, PagedAddress> listAddress,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List address", Tags = ["Addresses"])]
    [ProducesResponseType(typeof(ListAddressResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListAddressResponse>>> ListAddress(
        [FromQuery] ListAddressRequest request)
    {
        var result =
            await listAddress.Handle(new ListAddressQuery
            {
                Filter = request.Filter, PageToken = request.PageToken, MaxPageSize = request.MaxPageSize
            });

        var response = new ListAddressResponse
        {
            Results = mapper.Map<List<GetAddressResponse>>(result.Address), NextPageToken = result.NextPageToken
        };

        return Ok(response);
    }
}
