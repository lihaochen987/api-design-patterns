// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Review.ApplicationLayer;
using backend.Review.DomainModels;
using backend.Review.InfrastructureLayer;
using backend.Review.ReviewControllers;
using backend.Review.Services;
using backend.Shared;
using backend.Shared.FieldMask;
using backend.Shared.SqlFilter;
using Npgsql;

namespace backend.Review;

public class ReviewCompositionRoot
{
    private readonly ReviewFieldMaskConfiguration _reviewFieldMaskConfiguration;
    private readonly ReviewSqlFilterBuilder _reviewSqlFilterBuilder;
    private readonly QueryService<ReviewView> _reviewQueryService;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public ReviewCompositionRoot(
        IConfiguration configuration,
        SqlOperators sqlOperators,
        IFieldMaskConverterFactory fieldMaskConverterFactory)
    {
        ReviewColumnMapper reviewColumnMapper = new();
        SqlFilterParser reviewSqlFilterParser = new(reviewColumnMapper, sqlOperators);
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ReviewMappingProfile>(); });

        _reviewFieldMaskConfiguration = new ReviewFieldMaskConfiguration();
        _reviewSqlFilterBuilder = new ReviewSqlFilterBuilder(reviewSqlFilterParser);
        _reviewQueryService = new QueryService<ReviewView>();
        _fieldMaskConverterFactory = fieldMaskConverterFactory;
        _mapper = mapperConfig.CreateMapper();
        _configuration = configuration;
    }

    private IReviewRepository CreateReviewRepository()
    {
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        return new ReviewRepository(dbConnection);
    }

    private IReviewViewRepository CreateReviewViewRepository()
    {
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        return new ReviewViewRepository(dbConnection, _reviewSqlFilterBuilder, _reviewQueryService);
    }

    private IReviewApplicationService CreateReviewApplicationService()
    {
        var repository = CreateReviewRepository();
        return new ReviewApplicationService(repository, _reviewFieldMaskConfiguration);
    }

    private IReviewViewApplicationService CreateReviewViewApplicationService()
    {
        var repository = CreateReviewViewRepository();
        return new ReviewViewApplicationService(repository);
    }

    private CreateReviewController CreateCreateReviewController()
    {
        var applicationService = CreateReviewApplicationService();
        return new CreateReviewController(applicationService, _mapper);
    }

    private DeleteReviewController CreateDeleteReviewController()
    {
        var applicationService = CreateReviewApplicationService();
        return new DeleteReviewController(applicationService);
    }

    private GetReviewController CreateGetReviewController()
    {
        var applicationService = CreateReviewViewApplicationService();
        return new GetReviewController(
            applicationService,
            _reviewFieldMaskConfiguration,
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
        var applicationService = CreateReviewApplicationService();
        return new ReplaceReviewController(applicationService, _mapper);
    }

    private UpdateReviewController CreateUpdateReviewController()
    {
        var applicationService = CreateReviewApplicationService();
        return new UpdateReviewController(applicationService, _mapper);
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

        services.AddSingleton(_reviewFieldMaskConfiguration);
    }
}
