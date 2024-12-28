// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.ReviewControllers;

[ApiController]
[Route("review")]
public class DeleteReviewController(IReviewApplicationService applicationService) : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a review", Tags = ["Reviews"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteReview(
        [FromRoute] long id,
        [FromQuery] DeleteReviewRequest request)
    {
        DomainModels.Review? review = await applicationService.GetReviewAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        await applicationService.DeleteReviewAsync(id);
        return NoContent();
    }
}
