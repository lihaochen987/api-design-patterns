// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using MapsterMapper;

namespace backend.Product.Services.Mappers;

public class ProductTypeMapper(IMapper mapper) : IProductTypeMapper
{
    public TResponse MapToResponse<TResponse>(DomainModels.Product product, ProductControllerMethod method)
    {
        string methodName = method.ToString();

        string responseTypeName = product.Category switch
        {
            Category.PetFood => $"{methodName}PetFoodResponse",
            Category.GroomingAndHygiene => $"{methodName}GroomingAndHygieneResponse",
            _ => $"{methodName}ProductResponse"
        };

        Type responseType = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(a => a.GetTypes())
                                .FirstOrDefault(t => t.Name == responseTypeName)
                            ?? throw new InvalidOperationException(
                                $"Response type {responseTypeName} not found in any loaded assembly");

        Type sourceType = product.GetType();
        TResponse response = (TResponse)mapper.Map(product, sourceType, responseType);

        return response;
    }

    public TResponse MapToResponse<TResponse>(ProductView productView, ProductControllerMethod method)
    {
        string methodName = method.ToString();
        Category category = Enum.Parse<Category>(productView.Category);

        string responseTypeName = category switch
        {
            Category.PetFood => $"{methodName}PetFoodResponse",
            Category.GroomingAndHygiene => $"{methodName}GroomingAndHygieneResponse",
            _ => $"{methodName}ProductResponse"
        };

        // Search for the type across all loaded assemblies
        Type responseType = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(a => a.GetTypes())
                                .FirstOrDefault(t => t.Name == responseTypeName)
                            ?? throw new InvalidOperationException(
                                $"Response type {responseTypeName} not found in any loaded assembly");

        TResponse response = (TResponse)mapper.Map(productView, typeof(ProductView), responseType);

        return response;
    }

    public DomainModels.Product MapFromRequest<TRequest>(TRequest request)
    {
        var categoryProperty = typeof(TRequest).GetProperty("Category");
        if (categoryProperty == null)
            throw new InvalidOperationException(
                $"Request type {typeof(TRequest).Name} does not have a Category property");

        string categoryValue = categoryProperty.GetValue(request)?.ToString()
                               ?? throw new InvalidOperationException("Category property is null");

        Category category = Enum.Parse<Category>(categoryValue);

        Type productType = category switch
        {
            Category.PetFood => typeof(PetFood),
            Category.GroomingAndHygiene => typeof(GroomingAndHygiene),
            _ => typeof(DomainModels.Product)
        };

        DomainModels.Product product =
            (DomainModels.Product)mapper.Map(request ?? throw new NullReferenceException(nameof(request)),
                typeof(TRequest), productType);

        return product;
    }
}
