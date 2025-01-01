// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer;
using backend.Review.DomainModels;
using backend.Review.InfrastructureLayer;
using backend.Review.ReviewControllers;
using backend.Review.Services;
using backend.Shared;
using SqlFilterBuilder = backend.Shared.SqlFilter.SqlFilterBuilder;

namespace backend.Review;

public static class ReviewServiceExtensions
{
    public static void AddReviewDependencies(this IServiceCollection services)
    {
        // Inject Review Controllers
        services.AddAutoMapper(typeof(ReviewMappingProfile));

        // Inject Review Application Layer
        services.AddScoped<IReviewApplicationService, ReviewApplicationService>();
        services.AddScoped<IReviewViewApplicationService, ReviewViewApplicationService>();

        // Inject Review Infrastructure Layer
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IReviewViewRepository, ReviewViewRepository>();

        // Inject Review Services
        services.AddSingleton<ReviewFieldMaskConfiguration>();
        services.AddSingleton<SqlFilterBuilder, ReviewSqlFilterBuilder>();
        services.AddSingleton<IColumnMapper, ReviewColumnMapper>();
        services.AddSingleton<ReviewSqlFilterBuilder>();
        services.AddSingleton<QueryService<ReviewView>>();
    }
}
