// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Review.ApplicationLayer.Commands.DeleteReview;
using backend.Review.ApplicationLayer.Commands.ReplaceReview;
using backend.Review.ApplicationLayer.Commands.UpdateReview;
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
            var createReviewWithLogging = new LoggingCommandHandlerDecorator<CreateReviewCommand>(createReview,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<CreateReviewCommand>>());
            var createReviewWithAudit =
                new AuditCommandHandlerDecorator<CreateReviewCommand>(createReviewWithLogging, dbConnection);
            var createReviewWithTransaction =
                new TransactionCommandHandlerDecorator<CreateReviewCommand>(createReviewWithAudit, dbConnection);

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
            var deleteReviewWithLogging = new LoggingCommandHandlerDecorator<DeleteReviewCommand>(deleteReview,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<DeleteReviewCommand>>());
            var deleteReviewWithAudit =
                new AuditCommandHandlerDecorator<DeleteReviewCommand>(deleteReviewWithLogging, dbConnection);
            var deleteReviewWithTransaction =
                new TransactionCommandHandlerDecorator<DeleteReviewCommand>(deleteReviewWithAudit, dbConnection);

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
            var replaceReviewWithLogging = new LoggingCommandHandlerDecorator<ReplaceReviewCommand>(replaceReview,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<ReplaceReviewCommand>>());
            var replaceReviewWithAudit =
                new AuditCommandHandlerDecorator<ReplaceReviewCommand>(replaceReviewWithLogging, dbConnection);
            var replaceReviewWithTransaction =
                new TransactionCommandHandlerDecorator<ReplaceReviewCommand>(replaceReviewWithAudit, dbConnection);

            return new ReplaceReviewController(
                getReviewWithValidation,
                replaceReviewWithTransaction,
                _mapper);
        }

        if (type == typeof(UpdateReviewController))
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

            // UpdateReview handler
            var updateReview = new UpdateReviewHandler(repository);
            var updateReviewWithLogging = new LoggingCommandHandlerDecorator<UpdateReviewCommand>(updateReview,
                _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<UpdateReviewCommand>>());
            var updateReviewWithAudit =
                new AuditCommandHandlerDecorator<UpdateReviewCommand>(updateReviewWithLogging, dbConnection);
            var updateReviewWithTransaction =
                new TransactionCommandHandlerDecorator<UpdateReviewCommand>(updateReviewWithAudit, dbConnection);

            return new UpdateReviewController(
                getReviewWithValidation,
                updateReviewWithTransaction,
                _mapper);
        }

        return null;
    }
}
