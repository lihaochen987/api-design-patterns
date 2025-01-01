// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
using backend.Product.ProductControllers;
using backend.Product.ProductPricingControllers;
using backend.Product.Services.ProductPricingServices;
using backend.Product.Services.ProductServices;
using backend.Shared;
using backend.Supplier.Services;
using SqlFilterBuilder = backend.Shared.SqlFilter.SqlFilterBuilder;

namespace backend.Product;

public static class ProductServiceExtensions
{
    public static void AddProductDependencies(this IServiceCollection services)
    {
        // Inject Product Controllers
        services.AddAutoMapper(typeof(ProductMappingProfile));
        services.AddSingleton<GetProductPricingExtensions>();
        services.AddSingleton<CreateProductExtensions>();
        services.AddSingleton<UpdateProductPricingExtensions>();

        // Inject Product Application Layer
        services.AddScoped<IProductApplicationService, ProductApplicationService>();
        services.AddScoped<IProductViewApplicationService, ProductViewApplicationService>();

        // Inject Product Infrastructure Layer
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductViewRepository, ProductViewRepository>();
        services.AddScoped<IProductPricingRepository, ProductPricingRepository>();

        // Inject Product Services
        services.AddSingleton<ProductFieldMaskConfiguration>();
        services.AddSingleton<ProductPricingFieldMaskConfiguration>();
        services.AddSingleton<UpdateProductTypeService>();
        services.AddSingleton<ProductPricingFieldMaskService>();
        services.AddSingleton<DimensionsFieldMaskService>();
        services.AddSingleton<QueryService<ProductView>>();
        services.AddSingleton<ProductFieldPaths>();
    }
}
