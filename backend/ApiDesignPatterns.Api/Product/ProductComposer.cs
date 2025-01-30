// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Commands.DeleteProduct;
using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ApplicationLayer.Queries.GetProductPricing;
using backend.Product.ApplicationLayer.Queries.GetProductView;
using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.InfrastructureLayer.Database.ProductPricing;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.ProductControllers;
using backend.Product.ProductPricingControllers;
using backend.Product.Services;
using backend.Product.Services.ProductPricingServices;
using backend.Product.Services.ProductServices;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.FieldMask;
using backend.Shared.FieldPath;
using backend.Shared.QueryHandler;
using backend.Shared.SqlFilter;
using Npgsql;

namespace backend.Product;

public class ProductComposer
{
    private readonly GetProductPricingExtensions _getProductPricingExtensions;
    private readonly CreateProductExtensions _createProductExtensions;
    private readonly UpdateProductPricingExtensions _updateProductPricingExtensions;
    private readonly IConfiguration _configuration;
    private readonly QueryService<ProductView> _productQueryService;
    private readonly IFieldPathAdapter _fieldPathAdapter;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;
    private readonly IMapper _mapper;
    private readonly UpdateProductTypeService _updateProductTypeService;
    private readonly ProductPricingFieldPaths _productPricingFieldPaths;
    private readonly ProductPricingFieldMaskConfiguration _productPricingFieldMaskConfiguration;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ProductSqlFilterBuilder _productSqlFilterBuilder;
    private readonly RecursiveValidator _recursiveValidator;

    public ProductComposer(
        IConfiguration configuration,
        IFieldPathAdapter fieldPathAdapter,
        IFieldMaskConverterFactory fieldMaskConverterFactory,
        ILoggerFactory loggerFactory,
        RecursiveValidator recursiveValidator)
    {
        _getProductPricingExtensions = new GetProductPricingExtensions();
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        TypeParser typeParser = new();
        _createProductExtensions = new CreateProductExtensions(typeParser);
        _updateProductPricingExtensions = new UpdateProductPricingExtensions();
        _configuration = configuration;
        _productQueryService = new QueryService<ProductView>();
        _fieldPathAdapter = fieldPathAdapter;
        _fieldMaskConverterFactory = fieldMaskConverterFactory;
        _mapper = mapperConfig.CreateMapper();
        ProductPricingFieldMaskService productPricingFieldMaskService = new();
        DimensionsFieldMaskService dimensionsFieldMaskService = new();
        ProductFieldMaskConfiguration productFieldMaskConfiguration =
            new(productPricingFieldMaskService, dimensionsFieldMaskService);
        _updateProductTypeService = new UpdateProductTypeService(productFieldMaskConfiguration);
        _productPricingFieldPaths = new ProductPricingFieldPaths();
        _productPricingFieldMaskConfiguration = new ProductPricingFieldMaskConfiguration();
        _loggerFactory = loggerFactory;
        _recursiveValidator = recursiveValidator;
        ProductColumnMapper productColumnMapper = new();
        SqlFilterParser productSqlFilterParser = new(productColumnMapper);
        _productSqlFilterBuilder = new ProductSqlFilterBuilder(productSqlFilterParser);
    }

    private IProductRepository CreateProductRepository()
    {
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        return new ProductRepository(dbConnection);
    }

    private IProductViewRepository CreateProductViewRepository()
    {
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        return new ProductViewRepository(dbConnection, _productQueryService, _productSqlFilterBuilder);
    }

    private IProductPricingRepository CreateProductPricingRepository()
    {
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        return new ProductPricingRepository(dbConnection);
    }

    private ICommandHandler<UpdateProductQuery> CreateUpdateProductHandler()
    {
        var repository = CreateProductRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandHandler = new UpdateProductHandler(repository, _updateProductTypeService);
        var auditCommandHandler = new AuditCommandHandlerDecorator<UpdateProductQuery>(commandHandler, dbConnection);
        var loggerCommandHandler = new LoggingCommandHandlerDecorator<UpdateProductQuery>(auditCommandHandler,
            _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<UpdateProductQuery>>());
        var transactionCommandHandler =
            new TransactionCommandHandlerDecorator<UpdateProductQuery>(loggerCommandHandler, dbConnection);
        return transactionCommandHandler;
    }

    private ICommandHandler<CreateProductQuery> CreateCreateProductHandler()
    {
        var repository = CreateProductRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandHandler = new CreateProductHandler(repository);
        var auditCommandHandler = new AuditCommandHandlerDecorator<CreateProductQuery>(commandHandler, dbConnection);
        var loggerCommandHandler = new LoggingCommandHandlerDecorator<CreateProductQuery>(auditCommandHandler,
            _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<CreateProductQuery>>());
        var transactionHandler =
            new TransactionCommandHandlerDecorator<CreateProductQuery>(loggerCommandHandler, dbConnection);
        return transactionHandler;
    }

    private ICommandHandler<ReplaceProductQuery> CreateReplaceProductHandler()
    {
        var repository = CreateProductRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandHandler = new ReplaceProductHandler(repository);
        var auditCommandHandler = new AuditCommandHandlerDecorator<ReplaceProductQuery>(commandHandler, dbConnection);
        var loggerCommandHandler = new LoggingCommandHandlerDecorator<ReplaceProductQuery>(auditCommandHandler,
            _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<ReplaceProductQuery>>());
        return loggerCommandHandler;
    }

    private ICommandHandler<DeleteProductQuery> CreateDeleteProductHandler()
    {
        var repository = CreateProductRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandService = new DeleteProductHandler(repository);
        var auditCommandService = new AuditCommandHandlerDecorator<DeleteProductQuery>(commandService, dbConnection);
        var loggerCommandHandler = new LoggingCommandHandlerDecorator<DeleteProductQuery>(auditCommandService,
            _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<DeleteProductQuery>>());
        return loggerCommandHandler;
    }

    private IQueryHandler<GetProductPricingQuery, ProductPricingView> CreateGetProductPricingHandler()
    {
        var repository = CreateProductPricingRepository();
        var queryHandler = new GetProductPricingHandler(repository);
        var loggerQueryHandler = new LoggingQueryHandlerDecorator<GetProductPricingQuery, ProductPricingView>(
            queryHandler,
            _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetProductPricingQuery, ProductPricingView>>());
        var validationQueryHandler =
            new ValidationQueryHandlerDecorator<GetProductPricingQuery, ProductPricingView>(loggerQueryHandler,
                _recursiveValidator);
        return validationQueryHandler;
    }

    private IQueryHandler<GetProductQuery, DomainModels.Product> CreateGetProductHandler()
    {
        var repository = CreateProductRepository();
        var queryHandler = new GetProductHandler(repository);
        var loggerQueryHandler = new LoggingQueryHandlerDecorator<GetProductQuery, DomainModels.Product>(queryHandler,
            _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetProductQuery, DomainModels.Product>>());
        var validationQueryHandler =
            new ValidationQueryHandlerDecorator<GetProductQuery, DomainModels.Product>(loggerQueryHandler,
                _recursiveValidator);
        return validationQueryHandler;
    }

    private IQueryHandler<GetProductViewQuery, ProductView> CreateGetProductViewHandler()
    {
        var repository = CreateProductViewRepository();
        var queryHandler = new GetProductViewHandler(repository);
        var loggerQueryHandler = new LoggingQueryHandlerDecorator<GetProductViewQuery, ProductView>(queryHandler,
            _loggerFactory.CreateLogger<LoggingQueryHandlerDecorator<GetProductViewQuery, ProductView>>());
        var validationQueryHandler =
            new ValidationQueryHandlerDecorator<GetProductViewQuery, ProductView>(loggerQueryHandler,
                _recursiveValidator);
        return validationQueryHandler;
    }

    private IQueryHandler<ListProductsQuery, (List<ProductView>, string?)> CreateListProductsHandler()
    {
        var repository = CreateProductViewRepository();
        var queryHandler = new ListProductsHandler(repository);
        var loggerQueryHandler = new LoggingQueryHandlerDecorator<ListProductsQuery, (List<ProductView>, string?)>(
            queryHandler,
            _loggerFactory
                .CreateLogger<LoggingQueryHandlerDecorator<ListProductsQuery, (List<ProductView>, string?)>>());
        var validationQueryHandler =
            new ValidationQueryHandlerDecorator<ListProductsQuery, (List<ProductView>, string?)>(loggerQueryHandler,
                _recursiveValidator);
        return validationQueryHandler;
    }

    private GetProductController CreateGetProductController()
    {
        var queryHandler = CreateGetProductViewHandler();
        return new GetProductController(queryHandler, _fieldPathAdapter, _fieldMaskConverterFactory, _mapper);
    }

    private UpdateProductController CreateUpdateProductController()
    {
        var queryHandler = CreateGetProductHandler();
        var commandHandler = CreateUpdateProductHandler();
        return new UpdateProductController(queryHandler, commandHandler, _mapper);
    }

    private CreateProductController CreateCreateProductController()
    {
        var commandHandler = CreateCreateProductHandler();
        return new CreateProductController(commandHandler, _createProductExtensions, _mapper);
    }

    private ReplaceProductController CreateReplaceProductController()
    {
        var queryHandler = CreateGetProductHandler();
        var commandHandler = CreateReplaceProductHandler();
        return new ReplaceProductController(queryHandler, commandHandler, _mapper);
    }

    private DeleteProductController CreateDeleteProductController()
    {
        var commandHandler = CreateDeleteProductHandler();
        var queryHandler = CreateGetProductHandler();
        return new DeleteProductController(commandHandler, queryHandler);
    }

    private ListProductsController CreateListProductsController()
    {
        var queryHandler = CreateListProductsHandler();
        return new ListProductsController(queryHandler, _mapper);
    }

    private GetProductPricingController CreateGetProductPricingController()
    {
        var queryHandler = CreateGetProductPricingHandler();
        return new GetProductPricingController(queryHandler, _productPricingFieldPaths, _getProductPricingExtensions,
            _fieldMaskConverterFactory);
    }

    private UpdateProductPricingController CreateUpdateProductPricingController()
    {
        var repository = CreateProductRepository();
        return new UpdateProductPricingController(repository, _productPricingFieldMaskConfiguration,
            _updateProductPricingExtensions);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IProductRepository>(_ => CreateProductRepository());
        services.AddScoped<IProductViewRepository>(_ => CreateProductViewRepository());
        services.AddScoped<IProductPricingRepository>(_ => CreateProductPricingRepository());

        // Command Handlers
        services.AddScoped<ICommandHandler<UpdateProductQuery>>(_ => CreateUpdateProductHandler());
        services.AddScoped<ICommandHandler<CreateProductQuery>>(_ => CreateCreateProductHandler());
        services.AddScoped<ICommandHandler<ReplaceProductQuery>>(_ => CreateReplaceProductHandler());
        services.AddScoped<ICommandHandler<DeleteProductQuery>>(_ => CreateDeleteProductHandler());

        // Query Handlers
        services.AddScoped<IQueryHandler<GetProductQuery, DomainModels.Product>>(_ => CreateGetProductHandler());
        services.AddScoped<IQueryHandler<ListProductsQuery, (List<ProductView>, string?)>>(_ =>
            CreateListProductsHandler());
        services.AddScoped<IQueryHandler<GetProductViewQuery, ProductView>>(_ => CreateGetProductViewHandler());

        // Controllers
        services.AddScoped<GetProductController>(_ => CreateGetProductController());
        services.AddScoped<UpdateProductController>(_ => CreateUpdateProductController());
        services.AddScoped<CreateProductController>(_ => CreateCreateProductController());
        services.AddScoped<ReplaceProductController>(_ => CreateReplaceProductController());
        services.AddScoped<DeleteProductController>(_ => CreateDeleteProductController());
        services.AddScoped<GetProductPricingController>(_ => CreateGetProductPricingController());
        services.AddScoped<UpdateProductPricingController>(_ => CreateUpdateProductPricingController());
        services.AddScoped<ListProductsController>(_ => CreateListProductsController());

        services.AddSingleton<CreateProductExtensions>();
    }
}
