// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ProductControllers;
using backend.Product.Services;
using backend.Product.Services.Mappers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class UpdateProductControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly IQueryHandler<GetProductQuery, DomainModels.Product?> MockGetProductHandler;
    protected readonly ICommandHandler<UpdateProductCommand> MockUpdateProductHandler;

    protected UpdateProductControllerTestBase()
    {
        MockGetProductHandler = Mock.Of<IQueryHandler<GetProductQuery, DomainModels.Product?>>();
        MockUpdateProductHandler = Mock.Of<ICommandHandler<UpdateProductCommand>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected UpdateProductController UpdateProductController()
    {
        return new UpdateProductController(
            MockGetProductHandler,
            MockUpdateProductHandler,
            Mapper);
    }
}
