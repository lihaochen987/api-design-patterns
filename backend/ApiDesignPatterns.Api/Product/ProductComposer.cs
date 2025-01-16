// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ApplicationLayer.CreateProduct;
using backend.Product.ApplicationLayer.DeleteProduct;
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
using backend.Shared.CommandService;
using backend.Shared.FieldMask;
using backend.Shared.FieldPath;
using Microsoft.EntityFrameworkCore;

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

    public ProductComposer(
        IConfiguration configuration,
        IFieldPathAdapter fieldPathAdapter,
        IFieldMaskConverterFactory fieldMaskConverterFactory)
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

    private ICommandService<UpdateProduct> CreateUpdateProductService()
    {
        var repository = CreateProductRepository();
        return new UpdateProductService(repository, _updateProductTypeService);
    }

    private ICommandService<CreateProduct> CreateCreateProductService()
    {
        var repository = CreateProductRepository();
        return new CreateProductService(repository);
    }

    private ICommandService<ReplaceProduct> CreateReplaceProductService()
    {
        var repository = CreateProductRepository();
        return new ReplaceProductService(repository);
    }

    private ICommandService<DeleteProduct> CreateDeleteProductService()
    {
        var repository = CreateProductRepository();
        return new DeleteProductService(repository);
    }

    private IProductQueryApplicationService CreateProductQueryApplicationService()
    {
        var repository = CreateProductRepository();
        return new ProductQueryApplicationService(repository);
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
        var applicationService = CreateProductQueryApplicationService();
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
        var applicationService = CreateProductQueryApplicationService();
        var commandService = CreateReplaceProductService();
        return new ReplaceProductController(applicationService, commandService, _mapper);
    }

    private DeleteProductController CreateDeleteProductController()
    {
        var commandService = CreateDeleteProductService();
        var applicationService = CreateProductQueryApplicationService();
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
        services.AddScoped<ICommandService<UpdateProduct>>(_ => CreateUpdateProductService());
        services.AddScoped<ICommandService<CreateProduct>>(_ => CreateCreateProductService());
        services.AddScoped<ICommandService<ReplaceProduct>>(_ => CreateReplaceProductService());
        services.AddScoped<ICommandService<DeleteProduct>>(_ => CreateDeleteProductService());

        // Application Services
        services.AddScoped<IProductQueryApplicationService>(_ => CreateProductQueryApplicationService());
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
