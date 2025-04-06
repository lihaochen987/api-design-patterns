using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ApplicationLayer.Commands.UpdateListProductsStaleness;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.ApplicationLayer.Queries.MapListProductsResponse;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using backend.Shared.QueryProcessor;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[Route("products")]
[ApiController]
public class ListProductsController(
    IQueryProcessor queries,
    ICommandHandler<UpdateListProductStalenessCommand> updateListProductStaleness,
    ICommandHandler<PersistListProductsToCacheCommand> persistListProductsToCache,
    CacheStalenessOptions stalenessOptions)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List products", Tags = ["Products"])]
    [ProducesResponseType(typeof(ListProductsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        var getListProductsFromCacheQuery =
            new GetListProductsFromCacheQuery { Request = request, CheckRate = stalenessOptions.CheckRate };
        CacheQueryResult cachedResult = await queries.Process(getListProductsFromCacheQuery);

        if (cachedResult is { ProductsResponse: not null, SelectedForStalenessCheck: false })
        {
            return Ok(cachedResult.ProductsResponse);
        }

        var listProductsQuery = new ListProductsQuery
        {
            Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = request.PageToken
        };
        PagedProducts result = await queries.Process(listProductsQuery);

        var mapListProductsResponseQuery = new MapListProductsResponseQuery { PagedProducts = result };
        ListProductsResponse response = await queries.Process(mapListProductsResponseQuery);

        if (cachedResult is { SelectedForStalenessCheck: true, ProductsResponse: not null })
        {
            await updateListProductStaleness.Handle(new UpdateListProductStalenessCommand
            {
                FreshResult = response,
                CachedResult = cachedResult.ProductsResponse,
                StalenessOptions = stalenessOptions
            });
        }

        await persistListProductsToCache.Handle(new PersistListProductsToCacheCommand
        {
            CacheKey = cachedResult.CacheKey, Expiry = TimeSpan.FromSeconds(1), Products = response
        });

        return Ok(response);
    }
}
