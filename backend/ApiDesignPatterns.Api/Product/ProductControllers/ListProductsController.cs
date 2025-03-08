using AutoMapper;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.DomainModels.Enums;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[Route("products")]
[ApiController]
public class ListProductsController(
    IQueryHandler<ListProductsQuery, PagedProducts> listProducts,
    IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> getListProductsFromCache,
    ICommandHandler<PersistListProductsToCacheCommand> persistListProductsToCache,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List products", Tags = ["Products"])]
    [ProducesResponseType(typeof(ListProductsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        CacheQueryResult? cachedResult =
            await getListProductsFromCache.Handle(new GetListProductsFromCacheQuery { Request = request });

        if (cachedResult?.ProductsResponse != null)
        {
            return Ok(cachedResult.ProductsResponse);
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

        await persistListProductsToCache.Handle(new PersistListProductsToCacheCommand
        {
            CacheKey = cachedResult!.cacheKey, Expiry = TimeSpan.FromMinutes(10), Products = response
        });

        return Ok(response);
    }
}
