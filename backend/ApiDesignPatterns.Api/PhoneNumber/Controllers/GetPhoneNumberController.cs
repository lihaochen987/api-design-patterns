// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.PhoneNumber.ApplicationLayer.Queries.GetPhoneNumberView;
using backend.PhoneNumber.DomainModels;
using backend.Product.Controllers.Product;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.PhoneNumber.Controllers;

[ApiController]
[Route("phoneNumber")]
public class GetPhoneNumberController(
    IAsyncQueryHandler<GetPhoneNumberViewQuery, PhoneNumberView?> getPhoneNumberView,
    IFieldMaskConverterFactory fieldMaskConverterFactory,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get a phone number", Tags = ["PhoneNumbers"])]
    [ProducesResponseType(typeof(GetPhoneNumberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetProductResponse>> GetPhoneNumber(
        [FromRoute] long id,
        [FromQuery] GetPhoneNumberRequest request)
    {
        var phoneNumberView = await getPhoneNumberView.Handle(new GetPhoneNumberViewQuery { Id = id });
        if (phoneNumberView == null)
        {
            return NotFound();
        }

        var response = mapper.Map<GetPhoneNumberResponse>(phoneNumberView);

        var converter = fieldMaskConverterFactory.Create(request.FieldMask);
        JsonSerializerSettings settings = new() { Converters = new List<JsonConverter> { converter } };
        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
