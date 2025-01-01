// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ProductControllers;
using backend.Review.ApplicationLayer;
using backend.Review.DomainModels;
using backend.Review.Services;
using backend.Shared.FieldMask;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.ReviewControllers;

[ApiController]
[Route("review")]
public class GetReviewController(
    IReviewViewApplicationService reviewViewApplicationService,
    ReviewFieldMaskConfiguration maskConfiguration,
    FieldMaskConverterFactory fieldMaskConverterFactory,
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
        ReviewView? reviewView = await reviewViewApplicationService.GetReviewView(id);
        if (reviewView == null)
        {
            return NotFound();
        }

        var response = mapper.Map<GetReviewResponse>(reviewView);

        var converter = fieldMaskConverterFactory.Create(request.FieldMask, maskConfiguration.ReviewFieldPaths);
        JsonSerializerSettings settings = new() { Converters = new List<JsonConverter> { converter } };
        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
