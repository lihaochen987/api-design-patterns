// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Review.ApplicationLayer.Queries.GetReviewView;
using backend.Review.DomainModels;
using backend.Review.InfrastructureLayer.Database.ReviewView;
using backend.Review.ReviewControllers;
using backend.Review.Services;
using backend.Shared;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;

namespace backend.Review;

public class ReviewControllerActivator : BaseControllerActivator
{
    private readonly QueryService<ReviewView> _reviewQueryService;
    private readonly SqlFilterBuilder _reviewSqlFilterBuilder;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly IMapper _mapper;

    public ReviewControllerActivator(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration)
    {
        _reviewQueryService = new QueryService<ReviewView>();

        ReviewColumnMapper reviewColumnMapper = new();
        _reviewSqlFilterBuilder = new SqlFilterBuilder(reviewColumnMapper);

        _loggerFactory = loggerFactory;

        ReviewFieldPaths reviewFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(reviewFieldPaths.ValidPaths);

        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ReviewMappingProfile>(); });
        _mapper = mapperConfig.CreateMapper();
    }

    public override object? Create(ControllerContext context)
    {
        Type type = context.ActionDescriptor.ControllerTypeInfo.AsType();

        if (type == typeof(GetReviewController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewViewRepository(dbConnection, _reviewSqlFilterBuilder, _reviewQueryService);

            // GetReviewView handler
            var getReviewView = new GetReviewViewHandler(repository);
            var getReviewViewWithLogging = new LoggingQueryHandlerDecorator<GetReviewViewQuery, ReviewView>(
                getReviewView,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetReviewViewQuery, ReviewView>>());

            return new GetReviewController(
                getReviewViewWithLogging,
                _fieldMaskConverterFactory,
                _mapper);
        }

        return null;
    }
}
