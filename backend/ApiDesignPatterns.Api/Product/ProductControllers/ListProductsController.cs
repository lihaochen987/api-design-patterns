using AutoMapper;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.DomainModels.Enums;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[Route("products")]
[ApiController]
public class ListProductsController(
    IQueryHandler<ListProductsQuery, PagedProducts> listProducts,
    IQueryHandler<GetListProductsFromCacheQuery, ListProductsResponse> getListProductsFromCache,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List products", Tags = ["Products"])]
    [ProducesResponseType(typeof(ListProductsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        string cacheKey = GenerateCacheKey(request);

        ListProductsResponse? cachedResult =
            await getListProductsFromCache.Handle(new GetListProductsFromCacheQuery { CacheKey = cacheKey });

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        PagedProducts? result = await listProducts.Handle(new ListProductsQuery
        {
            Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = request.PageToken
        });

        if (result == null)
        {
            return NotFound();
        }

        IEnumerable<GetProductResponse> productResponses = result.Products.Select(product =>
            Enum.Parse<Category>(product.Category) switch
            {
                Category.PetFood => mapper.Map<GetPetFoodResponse>(product),
                Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(product),
                _ => mapper.Map<GetProductResponse>(product)
            }).ToList();

        ListProductsResponse response = new() { Results = productResponses, NextPageToken = result.NextPageToken };

        return Ok(response);
    }

    private static string GenerateCacheKey(ListProductsRequest request)
    {
        var keyParts = new List<string> { "products", $"maxsize:{request.MaxPageSize}" };

        if (!string.IsNullOrEmpty(request.PageToken))
        {
            keyParts.Add($"page-token:{request.PageToken}");
        }

        if (!string.IsNullOrEmpty(request.Filter))
        {
            string normalizedFilter = request.Filter.Trim().ToLowerInvariant();
            keyParts.Add($"filter:{normalizedFilter}");
        }

        return string.Join(":", keyParts);
    }
}
