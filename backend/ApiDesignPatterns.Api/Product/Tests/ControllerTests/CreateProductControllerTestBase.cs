// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.CacheCreateProductResponse;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Queries.GetCreateProductFromCache;
using backend.Product.Controllers.Product;
using backend.Product.Services.Mappers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.Shared.QueryProcessor;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class CreateProductControllerTestBase
{
    protected readonly Mock<IQueryProcessor> MockQueryProcessor;

    protected readonly ICommandHandler<CreateProductCommand> CreateProduct =
        Mock.Of<ICommandHandler<CreateProductCommand>>();

    protected readonly ICommandHandler<CacheCreateProductResponseCommand> CacheCreateProductResponse =
        Mock.Of<ICommandHandler<CacheCreateProductResponseCommand>>();

    protected readonly IMapper Mapper;

    protected readonly IProductTypeMapper ProductTypeMapper;

    protected CreateProductControllerTestBase()
    {
        MockQueryProcessor = new Mock<IQueryProcessor>();
        var config = new TypeAdapterConfig();
        config.RegisterProductMappings();
        Mapper = new Mapper(config);
        ProductTypeMapper = new ProductTypeMapper(Mapper);

        // GetCreateProductFromCache
        Mock<IAsyncQueryHandler<GetCreateProductFromCacheQuery, GetCreateProductFromCacheResult>>
            getCreateProductFromCacheQuery = new();
        MockQueryProcessor
            .Setup(qp => qp.Process(It.IsAny<GetCreateProductFromCacheQuery>()))
            .Returns<GetCreateProductFromCacheQuery>(query =>
                getCreateProductFromCacheQuery.Object.Handle(query));
    }

    protected CreateProductController GetCreateProductController()
    {
        return new CreateProductController(
            MockQueryProcessor.Object,
            CreateProduct,
            CacheCreateProductResponse,
            ProductTypeMapper);
    }
}
