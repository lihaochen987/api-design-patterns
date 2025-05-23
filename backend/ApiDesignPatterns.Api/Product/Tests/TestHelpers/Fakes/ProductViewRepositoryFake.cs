using System.Collections.ObjectModel;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared;

namespace backend.Product.Tests.TestHelpers.Fakes;

public class ProductViewRepositoryFake(
    PaginateService<ProductView> paginateService)
    : Collection<ProductView>, IProductViewRepository
{
    public void AddProductView(long id)
    {
        var productView = new ProductViewTestDataBuilder().WithId(id).Build();
        Add(productView);
    }

    public void AddProductView(long id, Category category)
    {
        var productView = new ProductViewTestDataBuilder().WithId(id).WithCategory(category).Build();
        Add(productView);
    }

    public Task<ProductView?> GetProductView(long id)
    {
        ProductView? productView = this.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(productView);
    }

    public Task<PagedProducts> ListProductsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var query = this.AsEnumerable();

        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenProduct))
        {
            query = query.Where(s => s.Id > lastSeenProduct);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            if (filter.Contains("Category =="))
            {
                string value = filter.Split('"')[1];
                query = query.Where(s => s.Category == value);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        var products = query
            .OrderBy(s => s.Id)
            .Take(maxPageSize + 1)
            .ToList();

        List<ProductView> paginatedProducts =
            paginateService.Paginate(products, maxPageSize, out string? nextPageToken);

        return Task.FromResult(new PagedProducts(paginatedProducts, nextPageToken, Count));
    }

    public Task<List<ProductView>> GetProductsByIds(List<long> productIds)
    {
        var productViews = this.Where(x => productIds.Contains(x.Id)).ToList();
        return Task.FromResult(productViews);
    }
}
