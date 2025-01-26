// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer;
using backend.Review.ApplicationLayer.Commands.DeleteReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.ReviewControllers;

[ApiController]
[Route("review")]
public class DeleteReviewController(
    IQueryHandler<GetReviewQuery, DomainModels.Review> getReview,
    ICommandHandler<DeleteReviewQuery> deleteReview)
    : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a review", Tags = ["Reviews"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteReview(
        [FromRoute] long id,
        [FromQuery] DeleteReviewRequest request)
    {
        DomainModels.Review? review = await getReview.Handle(new GetReviewQuery { Id = id });
        if (review == null)
        {
            return NotFound();
        }

        await deleteReview.Handle(new DeleteReviewQuery { Id = id });
        return NoContent();
    }
}
