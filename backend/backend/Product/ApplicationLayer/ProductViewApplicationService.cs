using AutoMapper;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
using backend.Product.ProductControllers;
using backend.Product.Services;
using backend.Shared;
using Newtonsoft.Json;

namespace backend.Product.ApplicationLayer;

public class ProductViewApplicationService(
    IProductViewRepository repository,
    IMapper mapper,
    ProductFieldMaskConfiguration maskConfiguration)
    : IProductViewApplicationService
{
    public async Task<string?> GetProductView(long id, GetProductRequest request)
    {
        // Prepare
        ProductView? product = await repository.GetProductView(id);

        // Todo: Better handling but this will do for now
        if (product == null)
        {
            return null;
        }

        // Execute
        GetProductResponse response = product.Category switch
        {
            Category.PetFood => mapper.Map<GetPetFoodResponse>(product),
            Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(product),
            _ => mapper.Map<GetProductResponse>(product)
        };
        JsonSerializerSettings settings = new()
        {
            Converters = new List<JsonConverter>
            {
                new FieldMaskConverter(request.FieldMask, maskConfiguration.ProductFieldPaths)
            }
        };
        string json = JsonConvert.SerializeObject(response, settings);

        // Apply
        return json;
    }

    public async Task<(List<ProductView>, string?)> ListProductsAsync(ListProductsRequest request)
    {
        // Prepare
        (List<ProductView> products, string? nextPageToken) = await repository.ListProductsAsync(
            request.PageToken,
            request.Filter,
            request.MaxPageSize);

        // Apply
        return (products, nextPageToken);
    }
}
