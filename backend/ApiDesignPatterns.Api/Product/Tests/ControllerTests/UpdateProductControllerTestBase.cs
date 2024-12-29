// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ProductControllers;
using backend.Product.Services;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class UpdateProductControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly IProductApplicationService MockApplicationService;

    protected UpdateProductControllerTestBase()
    {
        MockApplicationService = Mock.Of<IProductApplicationService>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected UpdateProductController UpdateProductController()
    {
        return new UpdateProductController(
            MockApplicationService,
            Mapper);
    }
}
