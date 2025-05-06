using backend.Review.ApplicationLayer.Queries.ListReviews;
using backend.Shared.QueryHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.Controllers;

[Route("{parentId}/reviews")]
[ApiController]
public class ListReviewsController(
    IAsyncQueryHandler<ListReviewsQuery, PagedReviews> listReviews,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List reviews", Tags = ["Reviews"])]
    [ProducesResponseType(typeof(ListReviewsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListReviewsResponse>>> ListReviews(
        [FromQuery] ListReviewsRequest request,
        string parentId)
    {
        PagedReviews result =
            await listReviews.Handle(new ListReviewsQuery { ParentId = parentId, Request = request });

        ListReviewsResponse response = new()
        {
            Results = mapper.Map<List<GetReviewResponse>>(result.Reviews), NextPageToken = result.NextPageToken
        };

        return Ok(response);
    }
}
