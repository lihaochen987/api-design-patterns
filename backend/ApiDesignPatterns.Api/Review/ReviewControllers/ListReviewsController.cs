using AutoMapper;
using backend.Product.ProductControllers;
using backend.Review.ApplicationLayer;
using backend.Review.DomainModels.Views;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Review.ReviewControllers;

[Route("reviews")]
[ApiController]
public class ListReviewsController(
    IReviewViewApplicationService applicationService,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List reviews", Tags = ["Reviews"])]
    [ProducesResponseType(typeof(ListReviewsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListReviewsResponse>>> ListReviews(
        [FromQuery] ListReviewsRequest request)
    {
        (List<ReviewView> reviews, string? nextPageToken) = await applicationService.ListProductsAsync(request);

        ListReviewsResponse response = new()
        {
            Results = mapper.Map<List<GetReviewResponse>>(reviews), NextPageToken = nextPageToken
        };

        return Ok(response);
    }
}
