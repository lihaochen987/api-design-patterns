// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.Controllers.Product;
using backend.Product.Services.Mappers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class ReplaceProductControllerTestBase
{
    protected readonly IAsyncQueryHandler<GetProductQuery, DomainModels.Product?> GetProduct =
        Mock.Of<IAsyncQueryHandler<GetProductQuery, DomainModels.Product?>>();

    protected readonly ICommandHandler<ReplaceProductCommand> ReplaceProduct =
        Mock.Of<ICommandHandler<ReplaceProductCommand>>();

    protected readonly IMapper Mapper;

    protected readonly IProductTypeMapper ProductTypeMapper;

    protected ReplaceProductControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterProductMappings();
        Mapper = new Mapper(config);
        ProductTypeMapper = new ProductTypeMapper(Mapper);
    }

    protected ReplaceProductController GetReplaceProductController()
    {
        return new ReplaceProductController(
            GetProduct,
            ReplaceProduct,
            ProductTypeMapper);
    }
}
