using backend.Product.Controllers.Product;
using backend.Review.ApplicationLayer.Commands.ReplaceReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.Controllers;

[ApiController]
[Route("review")]
public class ReplaceReviewController(
    IAsyncQueryHandler<GetReviewQuery, DomainModels.Review?> getReview,
    ICommandHandler<ReplaceReviewCommand> replaceReview,
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

        await replaceReview.Handle(new ReplaceReviewCommand { Review = replacedReview });
        var response = mapper.Map<ReplaceReviewResponse>(replacedReview);
        return Ok(response);
    }
}
