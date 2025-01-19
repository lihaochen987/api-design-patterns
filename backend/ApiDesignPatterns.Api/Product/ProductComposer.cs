// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ApplicationLayer.CreateProduct;
using backend.Product.ApplicationLayer.DeleteProduct;
using backend.Product.ApplicationLayer.GetProduct;
using backend.Product.ApplicationLayer.ReplaceProduct;
using backend.Product.ApplicationLayer.UpdateProduct;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
using backend.Product.InfrastructureLayer.Database;
using backend.Product.ProductControllers;
using backend.Product.ProductPricingControllers;
using backend.Product.Services.ProductPricingServices;
using backend.Product.Services.ProductServices;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.FieldMask;
using backend.Shared.FieldPath;
using backend.Shared.QueryHandler;
using Microsoft.EntityFrameworkCore;
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

    public ProductComposer(
        IConfiguration configuration,
        IFieldPathAdapter fieldPathAdapter,
        IFieldMaskConverterFactory fieldMaskConverterFactory,
        ILoggerFactory loggerFactory)
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
    }

    private IProductRepository CreateProductRepository()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        var dbContext = new ProductDbContext(optionsBuilder.Options);
        return new ProductRepository(dbContext);
    }

    private IProductViewRepository CreateProductViewRepository()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        var dbContext = new ProductDbContext(optionsBuilder.Options);
        return new ProductViewRepository(dbContext, _productQueryService);
    }

    private IProductPricingRepository CreateProductPricingRepository()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        var dbContext = new ProductDbContext(optionsBuilder.Options);
        return new ProductPricingRepository(dbContext);
    }

    private ICommandHandler<UpdateProductQuery> CreateUpdateProductService()
    {
        var repository = CreateProductRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandService = new UpdateProductHandler(repository, _updateProductTypeService);
        var auditCommandService = new AuditCommandHandlerDecorator<UpdateProductQuery>(commandService, dbConnection);
        var logger = _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<UpdateProductQuery>>();
        return new LoggingCommandHandlerDecorator<UpdateProductQuery>(auditCommandService, logger);
    }

    private ICommandHandler<CreateProductQuery> CreateCreateProductService()
    {
        var repository = CreateProductRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandService = new CreateProductHandler(repository);
        var auditCommandService = new AuditCommandHandlerDecorator<CreateProductQuery>(commandService, dbConnection);
        var logger = _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<CreateProductQuery>>();
        return new LoggingCommandHandlerDecorator<CreateProductQuery>(auditCommandService, logger);
    }

    private ICommandHandler<ReplaceProductQuery> CreateReplaceProductService()
    {
        var repository = CreateProductRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandService = new ReplaceProductHandler(repository);
        var auditCommandService = new AuditCommandHandlerDecorator<ReplaceProductQuery>(commandService, dbConnection);
        var logger = _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<ReplaceProductQuery>>();
        return new LoggingCommandHandlerDecorator<ReplaceProductQuery>(auditCommandService, logger);
    }

    private ICommandHandler<DeleteProductQuery> CreateDeleteProductService()
    {
        var repository = CreateProductRepository();
        var dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var commandService = new DeleteProductHandler(repository);
        var auditCommandService = new AuditCommandHandlerDecorator<DeleteProductQuery>(commandService, dbConnection);
        var logger = _loggerFactory.CreateLogger<LoggingCommandHandlerDecorator<DeleteProductQuery>>();
        return new LoggingCommandHandlerDecorator<DeleteProductQuery>(auditCommandService, logger);
    }

    private IQueryHandler<GetProductQuery, DomainModels.Product> CreateGetProductHandler()
    {
        var repository = CreateProductRepository();
        return new GetProductHandler(repository);
    }

    private IProductViewQueryApplicationService CreateProductViewQueryApplicationService()
    {
        var repository = CreateProductViewRepository();
        return new ProductViewQueryApplicationService(repository);
    }

    private GetProductController CreateGetProductController()
    {
        var applicationService = CreateProductViewQueryApplicationService();
        return new GetProductController(applicationService, _fieldPathAdapter, _fieldMaskConverterFactory, _mapper);
    }

    private UpdateProductController CreateUpdateProductController()
    {
        var applicationService = CreateGetProductHandler();
        var commandService = CreateUpdateProductService();
        return new UpdateProductController(applicationService, commandService, _mapper);
    }

    private CreateProductController CreateCreateProductController()
    {
        var commandService = CreateCreateProductService();
        return new CreateProductController(commandService, _createProductExtensions, _mapper);
    }

    private ReplaceProductController CreateReplaceProductController()
    {
        var applicationService = CreateGetProductHandler();
        var commandService = CreateReplaceProductService();
        return new ReplaceProductController(applicationService, commandService, _mapper);
    }

    private DeleteProductController CreateDeleteProductController()
    {
        var commandService = CreateDeleteProductService();
        var applicationService = CreateGetProductHandler();
        return new DeleteProductController(commandService, applicationService);
    }

    private ListProductsController CreateListProductsController()
    {
        var applicationService = CreateProductViewQueryApplicationService();
        return new ListProductsController(applicationService, _mapper);
    }

    private GetProductPricingController CreateGetProductPricingController()
    {
        var repository = CreateProductPricingRepository();
        return new GetProductPricingController(repository, _productPricingFieldPaths, _getProductPricingExtensions,
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

        // Command Services
        services.AddScoped<ICommandHandler<UpdateProductQuery>>(_ => CreateUpdateProductService());
        services.AddScoped<ICommandHandler<CreateProductQuery>>(_ => CreateCreateProductService());
        services.AddScoped<ICommandHandler<ReplaceProductQuery>>(_ => CreateReplaceProductService());
        services.AddScoped<ICommandHandler<DeleteProductQuery>>(_ => CreateDeleteProductService());

        // Application Services
        services.AddScoped<IQueryHandler<GetProductQuery, DomainModels.Product>>(_ => CreateGetProductHandler());
        services.AddScoped<IProductViewQueryApplicationService>(_ => CreateProductViewQueryApplicationService());

        // Controllers
        services.AddScoped<GetProductController>(_ => CreateGetProductController());
        services.AddScoped<UpdateProductController>(_ => CreateUpdateProductController());
        services.AddScoped<CreateProductController>(_ => CreateCreateProductController());
        services.AddScoped<ReplaceProductController>(_ => CreateReplaceProductController());
        services.AddScoped<DeleteProductController>(_ => CreateDeleteProductController());
        services.AddScoped<GetProductPricingController>(_ => CreateGetProductPricingController());
        services.AddScoped<UpdateProductPricingController>(_ => CreateUpdateProductPricingController());
        services.AddScoped<ListProductsController>(_ => CreateListProductsController());
    }
}
