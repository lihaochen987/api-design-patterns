// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.


using AutoMapper;
using backend.Review.ApplicationLayer;
using backend.Review.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.ReviewControllers;

[ApiController]
[Route("review")]
public class UpdateReviewController(
    IReviewApplicationService applicationService,
    ReviewFieldMaskConfiguration maskConfiguration,
    IMapper mapper)
    : ControllerBase
{
    [HttpPatch("{id:long}")]
    [SwaggerOperation(Summary = "Update a review", Tags = ["Reviews"])]
    [ProducesResponseType(typeof(UpdateReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateReviewResponse>> UpdateReview(
        [FromRoute] long id,
        [FromBody] UpdateReviewRequest request)
    {
        DomainModels.Review? review = await applicationService.GetReviewAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        (long productId, decimal rating, string text) =
            maskConfiguration.GetUpdatedReviewValues(request, review);
        review.Replace(productId, rating, text);
        await applicationService.UpdateReviewAsync(review);

        var response = mapper.Map<UpdateReviewResponse>(review);
        return Ok(response);
    }
}
