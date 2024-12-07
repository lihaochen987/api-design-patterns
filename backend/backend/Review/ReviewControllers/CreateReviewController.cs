// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Review.ApplicationLayer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.ReviewControllers;

[ApiController]
[Route("review")]
public class CreateReviewController(
    CreateReviewExtensions extensions,
    IReviewApplicationService applicationService,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a review", Tags = ["Reviews"])]
    [ProducesResponseType(typeof(CreateReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateReviewResponse>> CreateReview([FromBody] CreateReviewRequest request)
    {
        DomainModels.Review review = extensions.ToEntity(request);
        await applicationService.CreateReviewAsync(review);

        var response = mapper.Map<CreateReviewResponse>(review);
        return CreatedAtAction(nameof(CreateReview), new { id = review.Id }, response);
    }
}
