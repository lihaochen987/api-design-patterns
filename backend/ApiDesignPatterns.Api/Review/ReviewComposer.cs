// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Review.ApplicationLayer;
using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Review.ApplicationLayer.Commands.DeleteReview;
using backend.Review.ApplicationLayer.Commands.ReplaceReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.DomainModels;
using backend.Review.InfrastructureLayer.Database.Review;
using backend.Review.InfrastructureLayer.Database.ReviewView;
using backend.Review.ReviewControllers;
using backend.Review.Services;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using backend.Supplier.Services;
using Npgsql;
using SqlFilterBuilder = backend.Shared.SqlFilterBuilder;

namespace backend.Review;

public class ReviewComposer
{
    private readonly ReviewFieldMaskConfiguration _reviewFieldMaskConfiguration;
    private readonly SqlFilterBuilder _reviewSqlFilterBuilder;
    private readonly QueryService<ReviewView> _reviewQueryService;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public ReviewComposer(
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
    {
        ReviewColumnMapper reviewColumnMapper = new();
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ReviewMappingProfile>(); });

        _reviewFieldMaskConfiguration = new ReviewFieldMaskConfiguration();
        _reviewSqlFilterBuilder = new SqlFilterBuilder(reviewColumnMapper);
        _reviewQueryService = new QueryService<ReviewView>();
        SupplierFieldPaths fieldPaths = new();
        _fieldMaskConverterFactory = new FieldMaskConverterFactory(fieldPaths.ValidPaths);
        _loggerFactory = loggerFactory;
        _mapper = mapperConfig.CreateMapper();
        _configuration = configuration;
    }

    private ReviewRepository CreateReviewRepository()
    {
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        return new ReviewRepository(dbConnection);
    }

    private ReviewViewRepository CreateReviewViewRepository()
    {
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        return new ReviewViewRepository(dbConnection, _reviewSqlFilterBuilder, _reviewQueryService);
    }

    private ReviewApplicationService CreateReviewApplicationService()
    {
        var repository = CreateReviewRepository();
        return new ReviewApplicationService(repository, _reviewFieldMaskConfiguration);
    }

    private ReviewViewApplicationService CreateReviewViewApplicationService()
    {
        var repository = CreateReviewViewRepository();
        return new ReviewViewApplicationService(repository);
    }

    private CreateReviewController CreateCreateReviewController()
    {
        var commandHandler = CreateCreateReviewHandler();
        return new CreateReviewController(commandHandler, _mapper);
    }

    private DeleteReviewController CreateDeleteReviewController()
    {
        var commandHandler = CreateDeleteReviewHandler();
        var queryHandler = CreateGetReviewHandler();
        return new DeleteReviewController(queryHandler, commandHandler);
    }

    private GetReviewController CreateGetReviewController()
    {
        var applicationService = CreateReviewViewApplicationService();
        return new GetReviewController(
            applicationService,
            _fieldMaskConverterFactory,
            _mapper);
    }

    private ListReviewsController CreateListReviewsController()
    {
        var applicationService = CreateReviewViewApplicationService();
        return new ListReviewsController(applicationService, _mapper);
    }

    private ReplaceReviewController CreateReplaceReviewController()
    {
        var queryHandler = CreateGetReviewHandler();
        var commandHandler = CreateReplaceReviewHandler();
        return new ReplaceReviewController(queryHandler, commandHandler, _mapper);
    }

    private UpdateReviewController CreateUpdateReviewController()
    {
        var applicationService = CreateReviewApplicationService();
        var queryHandler = CreateGetReviewHandler();
        return new UpdateReviewController(queryHandler, applicationService, _mapper);
    }

    private IQueryHandler<GetReviewQuery, DomainModels.Review> CreateGetReviewHandler()
    {
        var repository = CreateReviewRepository();
        var queryHandler = new GetReviewHandler(repository);
        var loggerQueryHandler = new LoggingQueryHandlerDecorator<GetReviewQuery, DomainModels.Review>(
            queryHandler,
            _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetReviewQuery, DomainModels.Review>>());
        return loggerQueryHandler;
    }

    private ICommandHandler<CreateReviewQuery> CreateCreateReviewHandler()
    {
        var repository = CreateReviewRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandService = new CreateReviewHandler(repository);
        var auditCommandService = new AuditCommandHandlerDecorator<CreateReviewQuery>(commandService, dbConnection);
        var loggerCommandHandler = new LoggingCommandHandlerDecorator<CreateReviewQuery>(auditCommandService,
            _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<CreateReviewQuery>>());
        return loggerCommandHandler;
    }

    private ICommandHandler<DeleteReviewQuery> CreateDeleteReviewHandler()
    {
        var repository = CreateReviewRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandService = new DeleteReviewHandler(repository);
        var auditCommandService = new AuditCommandHandlerDecorator<DeleteReviewQuery>(commandService, dbConnection);
        var loggerCommandHandler = new LoggingCommandHandlerDecorator<DeleteReviewQuery>(auditCommandService,
            _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<DeleteReviewQuery>>());
        return loggerCommandHandler;
    }

    private ICommandHandler<ReplaceReviewQuery> CreateReplaceReviewHandler()
    {
        var repository = CreateReviewRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandService = new ReplaceReviewHandler(repository);
        var auditCommandService = new AuditCommandHandlerDecorator<ReplaceReviewQuery>(commandService, dbConnection);
        var loggerCommandHandler = new LoggingCommandHandlerDecorator<ReplaceReviewQuery>(auditCommandService,
            _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<ReplaceReviewQuery>>());
        return loggerCommandHandler;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IReviewRepository>(_ => CreateReviewRepository());
        services.AddScoped<IReviewViewRepository>(_ => CreateReviewViewRepository());
        services.AddScoped<IReviewApplicationService>(_ => CreateReviewApplicationService());
        services.AddScoped<IReviewViewApplicationService>(_ => CreateReviewViewApplicationService());
        services.AddScoped<CreateReviewController>(_ => CreateCreateReviewController());
        services.AddScoped<DeleteReviewController>(_ => CreateDeleteReviewController());
        services.AddScoped<GetReviewController>(_ => CreateGetReviewController());
        services.AddScoped<ListReviewsController>(_ => CreateListReviewsController());
        services.AddScoped<ReplaceReviewController>(_ => CreateReplaceReviewController());
        services.AddScoped<UpdateReviewController>(_ => CreateUpdateReviewController());

        // Command Handlers
        services.AddScoped<ICommandHandler<CreateReviewQuery>>(_ => CreateCreateReviewHandler());
        services.AddScoped<ICommandHandler<DeleteReviewQuery>>(_ => CreateDeleteReviewHandler());
        services.AddScoped<ICommandHandler<ReplaceReviewQuery>>(_ => CreateReplaceReviewHandler());

        // Query Handlers
        services.AddScoped<IQueryHandler<GetReviewQuery, DomainModels.Review>>(_ => CreateGetReviewHandler());

        services.AddSingleton(_reviewFieldMaskConfiguration);
    }
}
