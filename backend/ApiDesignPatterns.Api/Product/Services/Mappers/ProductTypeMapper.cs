// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;

namespace backend.Product.Services.Mappers;

public class ProductTypeMapper(IMapper mapper) : IProductTypeMapper
{
    public TResponse MapToResponse<TResponse>(DomainModels.Product product)
    {
        Type baseType = typeof(TResponse);
        string baseTypeName = baseType.Name;

        if (!baseTypeName.EndsWith("ProductResponse"))
        {
            return mapper.Map<TResponse>(product);
        }

        string operationPrefix = baseTypeName.Replace("ProductResponse", "");

        string specificTypeName = product.Category switch
        {
            Category.PetFood => $"{operationPrefix}PetFoodResponse",
            Category.GroomingAndHygiene => $"{operationPrefix}GroomingAndHygieneResponse",
            _ => baseTypeName
        };

        if (specificTypeName == baseTypeName)
        {
            return mapper.Map<TResponse>(product);
        }

        Type? specificType = baseType.Assembly.GetType($"{baseType.Namespace}.{specificTypeName}");

        if (specificType == null)
        {
            return mapper.Map<TResponse>(product);
        }

        object specificResponse = mapper.Map(product, product.GetType(), specificType);
        return mapper.Map<TResponse>(specificResponse);
    }

    public TResponse MapToResponse<TResponse>(ProductView productView)
    {
        Type baseType = typeof(TResponse);
        string baseTypeName = baseType.Name;

        if (!baseTypeName.EndsWith("ProductResponse"))
        {
            return mapper.Map<TResponse>(productView);
        }

        string operationPrefix = baseTypeName.Replace("ProductResponse", "");

        string specificTypeName = Enum.Parse<Category>(productView.Category) switch
        {
            Category.PetFood => $"{operationPrefix}PetFoodResponse",
            Category.GroomingAndHygiene => $"{operationPrefix}GroomingAndHygieneResponse",
            _ => baseTypeName
        };

        if (specificTypeName == baseTypeName)
        {
            return mapper.Map<TResponse>(productView);
        }

        Type? specificType = baseType.Assembly.GetType($"{baseType.Namespace}.{specificTypeName}");

        if (specificType == null)
        {
            return mapper.Map<TResponse>(productView);
        }

        object specificResponse = mapper.Map(productView, productView.GetType(), specificType);
        return mapper.Map<TResponse>(specificResponse);
    }

    public DomainModels.Product MapFromRequest<TRequest>(TRequest request)
    {
        Type requestType = typeof(TRequest);

        var categoryProperty = requestType.GetProperty("Category");
        if (categoryProperty == null)
        {
            return mapper.Map<DomainModels.Product>(request);
        }

        object? categoryValue = categoryProperty.GetValue(request);
        if (categoryValue == null)
        {
            return mapper.Map<DomainModels.Product>(request);
        }

        Category category;
        switch (categoryValue)
        {
            case string categoryString:
            {
                if (!Enum.TryParse(categoryString, out category))
                {
                    return mapper.Map<DomainModels.Product>(request);
                }

                break;
            }
            case Category categoryEnum:
                category = categoryEnum;
                break;
            default:
                return mapper.Map<DomainModels.Product>(request);
        }

        Type targetType = category switch
        {
            Category.PetFood => typeof(PetFood),
            Category.GroomingAndHygiene => typeof(GroomingAndHygiene),
            _ => typeof(DomainModels.Product)
        };

        return (DomainModels.Product)mapper.Map(request, requestType, targetType);
    }
}
