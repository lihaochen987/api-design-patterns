// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.Controllers.Product;
using backend.Review.ApplicationLayer.Queries.GetReviewView;
using backend.Review.DomainModels;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.Controllers;

[ApiController]
[Route("review")]
public class GetReviewController(
    IAsyncQueryHandler<GetReviewViewQuery, ReviewView?> getReviewView,
    IFieldMaskConverterFactory fieldMaskConverterFactory,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get a review", Tags = ["Reviews"])]
    [ProducesResponseType(typeof(GetReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetProductResponse>> GetReview(
        [FromRoute] long id,
        [FromQuery] GetReviewRequest request)
    {
        ReviewView? reviewView = await getReviewView.Handle(new GetReviewViewQuery { Id = id });
        if (reviewView == null)
        {
            return NotFound();
        }

        var response = mapper.Map<GetReviewResponse>(reviewView);

        var converter = fieldMaskConverterFactory.Create(request.FieldMask);
        JsonSerializerSettings settings = new() { Converters = new List<JsonConverter> { converter } };
        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
