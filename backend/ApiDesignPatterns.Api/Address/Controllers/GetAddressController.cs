// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Queries.GetAddressView;
using backend.Address.DomainModels;
using backend.Product.Controllers.Product;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Address.Controllers;

[ApiController]
[Route("address")]
public class GetAddressController(
    IAsyncQueryHandler<GetAddressViewQuery, AddressView?> getAddressView,
    IFieldMaskConverterFactory fieldMaskConverterFactory,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get an address", Tags = ["Addresses"])]
    [ProducesResponseType(typeof(GetAddressResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetProductResponse>> GetAddress(
        [FromRoute] long id,
        [FromQuery] GetAddressRequest request)
    {
        var addressView = await getAddressView.Handle(new GetAddressViewQuery { Id = id });
        if (addressView == null)
        {
            return NotFound();
        }

        var response = mapper.Map<GetAddressResponse>(addressView);

        var converter = fieldMaskConverterFactory.Create(request.FieldMask);
        JsonSerializerSettings settings = new() { Converters = new List<JsonConverter> { converter } };
        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
