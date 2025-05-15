// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.


using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Review.ApplicationLayer.Commands.DeleteReview;
using backend.Review.ApplicationLayer.Commands.ReplaceReview;
using backend.Review.ApplicationLayer.Commands.UpdateReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.ApplicationLayer.Queries.GetReviewView;
using backend.Review.ApplicationLayer.Queries.ListReviews;
using backend.Review.Controllers;
using backend.Review.DomainModels;
using backend.Review.InfrastructureLayer.Database.Review;
using backend.Review.InfrastructureLayer.Database.ReviewView;
using backend.Review.Services;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.ControllerActivators;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using IMapper = MapsterMapper.IMapper;

namespace backend.Review;

public class ReviewControllerActivator : BaseControllerActivator
{
    private readonly PaginateService<ReviewView> _reviewPaginateService;
    private readonly SqlFilterBuilder _reviewSqlFilterBuilder;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly IMapper _mapper;

    public ReviewControllerActivator(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration)
    {
        _reviewPaginateService = new PaginateService<ReviewView>();

        ReviewColumnMapper reviewColumnMapper = new();
        _reviewSqlFilterBuilder = new SqlFilterBuilder(reviewColumnMapper);

        _loggerFactory = loggerFactory;

        ReviewFieldPaths reviewFieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(reviewFieldPaths.ValidPaths);

        var config = new TypeAdapterConfig();
        config.RegisterReviewMappings();
        _mapper = new Mapper(config);
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
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ReviewWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
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
            var getReviewHandler = new QueryDecoratorBuilder<GetReviewQuery, DomainModels.Review?>(
                    new GetReviewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ReviewRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // DeleteReview handler
            var deleteReviewHandler = new CommandDecoratorBuilder<DeleteReviewCommand>(
                    new DeleteReviewHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ReviewWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new DeleteReviewController(
                getReviewHandler,
                deleteReviewHandler);
        }


        if (type == typeof(GetReviewController))
        {
            var dbConnection = CreateDbConnection();
            TrackDisposable(context, dbConnection);
            var repository = new ReviewViewRepository(dbConnection, _reviewSqlFilterBuilder, _reviewPaginateService);

            // GetReviewView handler
            var getReviewViewHandler = new QueryDecoratorBuilder<GetReviewViewQuery, ReviewView?>(
                    new GetReviewViewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ReviewRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
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
            var repository = new ReviewViewRepository(dbConnection, _reviewSqlFilterBuilder, _reviewPaginateService);

            // ListReviews query handler
            var listReviewsHandler = new QueryDecoratorBuilder<ListReviewsQuery, PagedReviews>(
                    new ListReviewsHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ReviewRead)
                .WithLogging()
                .WithTransaction()
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
            var getReviewHandler = new QueryDecoratorBuilder<GetReviewQuery, DomainModels.Review?>(
                    new GetReviewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ReviewRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // ReplaceReview handler
            var replaceReviewHandler = new CommandDecoratorBuilder<ReplaceReviewCommand>(
                    new ReplaceReviewHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ReviewWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
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
            var getReviewHandler = new QueryDecoratorBuilder<GetReviewQuery, DomainModels.Review?>(
                    new GetReviewHandler(repository),
                    _loggerFactory,
                    dbConnection)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ReviewRead)
                .WithLogging()
                .WithValidation()
                .WithTransaction()
                .Build();

            // UpdateReview handler
            var updateReviewHandler = new CommandDecoratorBuilder<UpdateReviewCommand>(
                    new UpdateReviewHandler(repository),
                    dbConnection,
                    _loggerFactory)
                .WithCircuitBreaker(JitterUtility.AddJitter(TimeSpan.FromSeconds(30)), 3)
                .WithHandshaking()
                .WithTimeout(JitterUtility.AddJitter(TimeSpan.FromSeconds(5)))
                .WithBulkhead(BulkheadPolicies.ReviewWrite)
                .WithLogging()
                .WithAudit()
                .WithTransaction()
                .Build();

            return new UpdateReviewController(
                getReviewHandler,
                updateReviewHandler,
                _mapper);
        }

        return null;
    }
}
