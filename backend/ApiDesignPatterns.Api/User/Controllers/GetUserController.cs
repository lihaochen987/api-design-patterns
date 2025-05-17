// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Queries.GetUserView;
using backend.User.DomainModels;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.User.Controllers;

[ApiController]
[Route("user")]
public class GetUserController(
    IAsyncQueryHandler<GetUserViewQuery, UserView?> getUserView,
    IFieldMaskConverterFactory fieldMaskConverterFactory,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get a user", Tags = ["Users"])]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetProductResponse>> GetUser(
        [FromRoute] long id,
        [FromQuery] GetUserRequest request)
    {
        var userView = await getUserView.Handle(new GetUserViewQuery { Id = id });
        if (userView == null)
        {
            return NotFound();
        }

        var response = mapper.Map<GetUserResponse>(userView);

        var converter = fieldMaskConverterFactory.Create(request.FieldMask);
        JsonSerializerSettings settings = new() { Converters = new List<JsonConverter> { converter } };
        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
