// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using MapsterMapper;

namespace backend.Product.Services.Mappers;

public class ProductTypeMapper(IMapper mapper) : IProductTypeMapper
{
    private static ImmutableDictionary<string, Type> s_typeCache = ImmutableDictionary<string, Type>.Empty;

    private static readonly Dictionary<Category, string> s_categoryToResponseSuffix = new()
    {
        { Category.PetFood, "PetFood" }, { Category.GroomingAndHygiene, "GroomingAndHygiene" }
    };

    private static string GetResponseTypeName(Category category, string methodName)
    {
        string suffix = s_categoryToResponseSuffix.GetValueOrDefault(category, "Product");

        return $"{methodName}{suffix}Response";
    }

    public TResponse MapToResponse<TResponse>(DomainModels.Product product, ProductControllerMethod method)
    {
        string methodName = method.ToString();
        string responseTypeName = GetResponseTypeName(product.Category, methodName);
        Type sourceType = product.GetType();

        if (s_typeCache.TryGetValue(responseTypeName, out Type? cachedResponseType))
        {
            return (TResponse)mapper.Map(product, sourceType, cachedResponseType);
        }

        Type responseType = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(a => a.GetTypes())
                                .FirstOrDefault(t => t.Name == responseTypeName)
                            ?? throw new InvalidOperationException(
                                $"Response type {responseTypeName} not found in any loaded assembly");

        s_typeCache = s_typeCache.Add(responseTypeName, responseType);

        TResponse response = (TResponse)mapper.Map(product, sourceType, responseType);

        return response;
    }

    public TResponse MapToResponse<TResponse>(ProductView productView, ProductControllerMethod method)
    {
        string methodName = method.ToString();
        Category category = Enum.Parse<Category>(productView.Category);
        string responseTypeName = GetResponseTypeName(category, methodName);
        Type sourceType = productView.GetType();

        if (s_typeCache.TryGetValue(responseTypeName, out Type? cachedResponseType))
        {
            return (TResponse)mapper.Map(productView, sourceType, cachedResponseType);
        }

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
