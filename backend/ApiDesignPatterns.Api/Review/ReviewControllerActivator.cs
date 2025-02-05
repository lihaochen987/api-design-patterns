// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Review.ApplicationLayer.Commands.DeleteReview;
using backend.Review.ApplicationLayer.Commands.ReplaceReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.ApplicationLayer.Queries.GetReviewView;
using backend.Review.ApplicationLayer.Queries.ListReviews;
using backend.Review.DomainModels;
using backend.Review.InfrastructureLayer.Database.Review;
using backend.Review.InfrastructureLayer.Database.ReviewView;
using backend.Review.ReviewControllers;
using backend.Review.Services;
using backend.Shared;
using backend.Shared.CommandHandler;
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

        if (type == typeof(CreateReviewController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewRepository(dbConnection);

            // CreateReview handler
            var createReview = new CreateReviewHandler(repository);
            var createReviewWithLogging = new LoggingCommandHandlerDecorator<CreateReviewQuery>(createReview,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<CreateReviewQuery>>());
            var createReviewWithAudit =
                new AuditCommandHandlerDecorator<CreateReviewQuery>(createReviewWithLogging, dbConnection);
            var createReviewWithTransaction =
                new TransactionCommandHandlerDecorator<CreateReviewQuery>(createReviewWithAudit, dbConnection);

            return new CreateReviewController(
                createReviewWithTransaction,
                _mapper);
        }

        if (type == typeof(DeleteReviewController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewRepository(dbConnection);

            // GetReview handler
            var getReview = new GetReviewHandler(repository);
            var getReviewWithLogging = new LoggingQueryHandlerDecorator<GetReviewQuery, DomainModels.Review>(
                getReview,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetReviewQuery, DomainModels.Review>>());
            var getReviewWithValidation =
                new ValidationQueryHandlerDecorator<GetReviewQuery, DomainModels.Review>(getReviewWithLogging);

            // DeleteReview handler
            var deleteReview = new DeleteReviewHandler(repository);
            var deleteReviewWithLogging = new LoggingCommandHandlerDecorator<DeleteReviewQuery>(deleteReview,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<DeleteReviewQuery>>());
            var deleteReviewWithAudit =
                new AuditCommandHandlerDecorator<DeleteReviewQuery>(deleteReviewWithLogging, dbConnection);
            var deleteReviewWithTransaction =
                new TransactionCommandHandlerDecorator<DeleteReviewQuery>(deleteReviewWithAudit, dbConnection);

            return new DeleteReviewController(
                getReviewWithValidation,
                deleteReviewWithTransaction);
        }


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
            var getReviewViewWithValidation =
                new ValidationQueryHandlerDecorator<GetReviewViewQuery, ReviewView>(getReviewViewWithLogging);

            return new GetReviewController(
                getReviewViewWithValidation,
                _fieldMaskConverterFactory,
                _mapper);
        }

        if (type == typeof(ListReviewsController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewViewRepository(dbConnection, _reviewSqlFilterBuilder, _reviewQueryService);

            // ListReviews query handler
            var listReviews = new ListReviewsHandler(repository);
            var listReviewsWithLogging =
                new LoggingQueryHandlerDecorator<ListReviewsQuery, (List<ReviewView>, string?)>(
                    listReviews,
                    _loggerFactory
                        .CreateLogger<LoggingQueryHandlerDecorator<ListReviewsQuery, (List<ReviewView>, string?)>>());

            return new ListReviewsController(
                listReviewsWithLogging,
                _mapper);
        }

        if (type == typeof(ReplaceReviewController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewRepository(dbConnection);

            // GetReview handler
            var getReview = new GetReviewHandler(repository);
            var getReviewWithLogging = new LoggingQueryHandlerDecorator<GetReviewQuery, DomainModels.Review>(
                getReview,
                _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetReviewQuery, DomainModels.Review>>());
            var getReviewWithValidation =
                new ValidationQueryHandlerDecorator<GetReviewQuery, DomainModels.Review>(getReviewWithLogging);

            // ReplaceReview handler
            var replaceReview = new ReplaceReviewHandler(repository);
            var replaceReviewWithLogging = new LoggingCommandHandlerDecorator<ReplaceReviewQuery>(replaceReview,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<ReplaceReviewQuery>>());
            var replaceReviewWithAudit =
                new AuditCommandHandlerDecorator<ReplaceReviewQuery>(replaceReviewWithLogging, dbConnection);
            var replaceReviewWithTransaction =
                new TransactionCommandHandlerDecorator<ReplaceReviewQuery>(replaceReviewWithAudit, dbConnection);

            return new ReplaceReviewController(
                getReviewWithValidation,
                replaceReviewWithTransaction,
                _mapper);
        }

        return null;
    }
}
