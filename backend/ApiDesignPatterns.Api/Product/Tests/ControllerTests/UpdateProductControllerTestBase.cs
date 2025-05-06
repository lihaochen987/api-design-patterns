// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.Controllers.Product;
using backend.Product.Services.Mappers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class UpdateProductControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly IAsyncQueryHandler<GetProductQuery, DomainModels.Product?> MockGetProductHandler;
    protected readonly ICommandHandler<UpdateProductCommand> MockUpdateProductHandler;

    protected UpdateProductControllerTestBase()
    {
        MockGetProductHandler = Mock.Of<IAsyncQueryHandler<GetProductQuery, DomainModels.Product?>>();
        MockUpdateProductHandler = Mock.Of<ICommandHandler<UpdateProductCommand>>();
        var config = new TypeAdapterConfig();
        config.RegisterProductMappings();
        Mapper = new Mapper(config);
    }

    protected UpdateProductController UpdateProductController()
    {
        return new UpdateProductController(
            MockGetProductHandler,
            MockUpdateProductHandler,
            Mapper);
    }
}
