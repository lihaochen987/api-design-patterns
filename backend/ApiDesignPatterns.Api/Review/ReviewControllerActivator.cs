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
            var createReviewHandler = new CommandDecoratorBuilder<CreateReviewCommand>(
                    new CreateReviewHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithAudit()
                .WithLogging()
                .WithTransaction()
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .Build();

            return new CreateReviewController(
                createReviewHandler,
                _mapper);
        }

        if (type == typeof(DeleteReviewController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewRepository(dbConnection);

            // GetReview handler
            var getReviewHandler = new QueryDecoratorBuilder<GetReviewQuery, DomainModels.Review>(
                    new GetReviewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .WithCircuitBreaker()
                .Build();

            // DeleteReview handler
            var deleteReviewHandler = new CommandDecoratorBuilder<DeleteReviewCommand>(
                    new DeleteReviewHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithAudit()
                .WithLogging()
                .WithTransaction()
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .Build();

            return new DeleteReviewController(
                getReviewHandler,
                deleteReviewHandler);
        }


        if (type == typeof(GetReviewController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewViewRepository(dbConnection, _reviewSqlFilterBuilder, _reviewQueryService);

            // GetReviewView handler
            var getReviewViewHandler = new QueryDecoratorBuilder<GetReviewViewQuery, ReviewView>(
                    new GetReviewViewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .WithCircuitBreaker()
                .Build();

            return new GetReviewController(
                getReviewViewHandler,
                _fieldMaskConverterFactory,
                _mapper);
        }

        if (type == typeof(ListReviewsController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewViewRepository(dbConnection, _reviewSqlFilterBuilder, _reviewQueryService);

            // ListReviews query handler
            var listReviewsHandler = new QueryDecoratorBuilder<ListReviewsQuery, PagedReviews>(
                    new ListReviewsHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithLogging()
                .WithTransaction()
                .WithCircuitBreaker()
                .Build();


            return new ListReviewsController(
                listReviewsHandler,
                _mapper);
        }

        if (type == typeof(ReplaceReviewController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewRepository(dbConnection);

            // GetReview handler
            var getReviewHandler = new QueryDecoratorBuilder<GetReviewQuery, DomainModels.Review>(
                    new GetReviewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .WithCircuitBreaker()
                .Build();

            // ReplaceReview handler
            var replaceReviewHandler = new CommandDecoratorBuilder<ReplaceReviewCommand>(
                    new ReplaceReviewHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithAudit()
                .WithLogging()
                .WithTransaction()
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .Build();

            return new ReplaceReviewController(
                getReviewHandler,
                replaceReviewHandler,
                _mapper);
        }

        if (type == typeof(UpdateReviewController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewRepository(dbConnection);

            // GetReview handler
            var getReviewHandler = new QueryDecoratorBuilder<GetReviewQuery, DomainModels.Review>(
                    new GetReviewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .WithCircuitBreaker()
                .Build();

            // UpdateReview handler
            var updateReviewHandler = new CommandDecoratorBuilder<UpdateReviewCommand>(
                    new UpdateReviewHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithAudit()
                .WithLogging()
                .WithTransaction()
                .WithCircuitBreaker(TimeSpan.FromSeconds(30), 3)
                .Build();

            return new UpdateReviewController(
                getReviewHandler,
                updateReviewHandler,
                _mapper);
        }

        return null;
    }
}
