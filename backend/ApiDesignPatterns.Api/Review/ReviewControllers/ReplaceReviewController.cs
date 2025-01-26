using AutoMapper;
using backend.Product.ProductControllers;
using backend.Review.ApplicationLayer.Commands.ReplaceReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.ReviewControllers;

[ApiController]
[Route("review")]
public class ReplaceReviewController(
    IQueryHandler<GetReviewQuery, DomainModels.Review> getReview,
    ICommandHandler<ReplaceReviewQuery> replaceReview,
    IMapper mapper)
    : ControllerBase
{
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Replace a review", Tags = ["Reviews"])]
    [ProducesResponseType(typeof(ReplaceReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReplaceProductResponse>> ReplaceReview(
        [FromRoute] long id,
        [FromBody] ReplaceReviewRequest request)
    {
        DomainModels.Review? existingReview = await getReview.Handle(new GetReviewQuery { Id = id });
        if (existingReview == null)
        {
            return NotFound();
        }

        DomainModels.Review replacedReview = mapper.Map(request, existingReview);

        await replaceReview.Handle(new ReplaceReviewQuery { Review = replacedReview });
        var response = mapper.Map<ReplaceReviewResponse>(replacedReview);
        return Ok(response);
    }
}
