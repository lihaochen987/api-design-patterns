// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Shared.CommandHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.Controllers;

[ApiController]
[Route("{productId:long}/review")]
public class CreateReviewController(
    ICommandHandler<CreateReviewCommand> createReview,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a review", Tags = ["Reviews"])]
    [ProducesResponseType(typeof(CreateReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateReviewResponse>> CreateReview(
        [FromBody] CreateReviewRequest request,
        long productId)
    {
        var review = mapper.Map<DomainModels.Review>(request);
        await createReview.Handle(new CreateReviewCommand { Review = review, ProductId = productId });

        var response = mapper.Map<CreateReviewResponse>(review);
        return CreatedAtAction(
            "GetReview",
            "GetReview",
            new { id = review.Id },
            response);
    }
}
