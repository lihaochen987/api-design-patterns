using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ApplicationLayer.Commands.UpdateListProductsStaleness;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.ApplicationLayer.Queries.MapListProductsResponse;
using backend.Shared.Caching;
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
    IQueryHandler<MapListProductsResponseQuery, ListProductsResponse> mapListProducts,
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
        CacheQueryResult cachedResult =
            await getListProductsFromCache.Handle(
                new GetListProductsFromCacheQuery { Request = request, CheckRate = stalenessOptions.CheckRate });

        if (cachedResult is { ProductsResponse: not null, SelectedForStalenessCheck: false })
        {
            return Ok(cachedResult.ProductsResponse);
        }

        PagedProducts result = await listProducts.Handle(new ListProductsQuery
        {
            Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = request.PageToken
        });

        ListProductsResponse response =
            await mapListProducts.Handle(new MapListProductsResponseQuery { PagedProducts = result });

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
            CacheKey = cachedResult.CacheKey, Expiry = TimeSpan.FromMinutes(10), Products = response
        });

        return Ok(response);
    }
}
