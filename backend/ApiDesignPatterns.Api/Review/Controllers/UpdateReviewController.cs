// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.


using backend.Review.ApplicationLayer.Commands.UpdateReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.Controllers;

[ApiController]
[Route("review")]
public class UpdateReviewController(
    IAsyncQueryHandler<GetReviewQuery, DomainModels.Review?> getReview,
    ICommandHandler<UpdateReviewCommand> updateReview,
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
        DomainModels.Review? existingReview = await getReview.Handle(new GetReviewQuery { Id = id });

        if (existingReview == null)
        {
            return NotFound();
        }

        await updateReview.Handle(new UpdateReviewCommand { Request = request, Review = existingReview });

        var updatedReview = await getReview.Handle(new GetReviewQuery { Id = id });

        if (updatedReview == null)
        {
            return BadRequest();
        }

        var response = mapper.Map<UpdateReviewResponse>(updatedReview);
        return Ok(response);
    }
}
