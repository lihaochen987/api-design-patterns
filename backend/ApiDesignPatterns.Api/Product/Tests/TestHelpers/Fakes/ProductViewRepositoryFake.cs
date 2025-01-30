using System.Collections.ObjectModel;
using System.Linq.Expressions;
using AutoFixture;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared;

namespace backend.Product.Tests.TestHelpers.Fakes;

public class ProductViewRepositoryFake(
    QueryService<ProductView> queryService)
    : Collection<ProductView>, IProductViewRepository
{
    public void AddProductView(int id, Category category)
    {
        var productView = new ProductViewTestDataBuilder().WithId(id).WithCategory(category).Build();
        Add(productView);
    }
    public Task<ProductView?> GetProductView(long id)
    {
        ProductView? productView = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(productView);
    }

    public Task<(List<ProductView>, string?)> ListProductsAsync(string? pageToken, string? filter, int maxPageSize)
    {
        IEnumerable<ProductView> query = this.AsEnumerable();

        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenProductId))
        {
            query = query.Where(p => p.Id > lastSeenProductId);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            Expression<Func<ProductView, bool>> filterExpression = queryService.BuildFilterExpression(filter);
            query = query.Where(filterExpression.Compile());
        }

        List<ProductView> products = query.OrderBy(p => p.Id).ToList();

        List<ProductView> paginatedProducts = queryService.Paginate(products, maxPageSize, out string? nextPageToken);

        return Task.FromResult((paginatedProducts, nextPageToken));
    }
}
