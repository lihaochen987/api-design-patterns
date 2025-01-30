// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;
using backend.Shared.SqlFilter;
using backend.Supplier.ApplicationLayer;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer;
using backend.Supplier.InfrastructureLayer.Database.Supplier;
using backend.Supplier.InfrastructureLayer.Database.SupplierView;
using backend.Supplier.Services;
using backend.Supplier.SupplierControllers;

namespace backend.Supplier;

public static class SupplierServiceExtensions
{
    public static void AddSupplierDependencies(this IServiceCollection services)
    {
        // Inject Supplier Controllers
        services.AddAutoMapper(typeof(SupplierMappingProfile));

        // Inject Supplier Application Layer
        services.AddScoped<ISupplierApplicationService, SupplierApplicationService>();
        services.AddScoped<ISupplierViewApplicationService, SupplierViewApplicationService>();

        // Inject Supplier Infrastructure Layer
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ISupplierViewRepository, SupplierViewRepository>();

        // Inject Supplier Services
        services.AddSingleton<SupplierFieldMaskConfiguration>();
        services.AddSingleton<IColumnMapper, SupplierColumnMapper>();
        services.AddSingleton<SupplierFieldPaths>();
        services.AddSingleton<SupplierValueObjectUpdater>();
        services.AddScoped<SupplierDataWriter>();
        services.AddSingleton<QueryService<SupplierView>>();
    }
}
