// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ApplicationLayer.UpdateProduct;
using backend.Product.ProductControllers;
using backend.Product.Services;
using backend.Shared;
using backend.Shared.CommandHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class UpdateProductControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly IProductQueryApplicationService MockProductQueryService;
    protected readonly ICommandHandler<UpdateProduct> MockUpdateProductHandler;

    protected UpdateProductControllerTestBase()
    {
        MockProductQueryService = Mock.Of<IProductQueryApplicationService>();
        MockUpdateProductHandler = Mock.Of<ICommandHandler<UpdateProduct>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected UpdateProductController UpdateProductController()
    {
        return new UpdateProductController(
            MockProductQueryService,
            MockUpdateProductHandler,
            Mapper);
    }
}
