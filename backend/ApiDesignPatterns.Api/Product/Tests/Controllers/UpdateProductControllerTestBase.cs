// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ProductControllers;
using backend.Product.Services;
using Moq;

namespace backend.Product.Tests.Controllers;

public abstract class UpdateProductControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly IProductApplicationService MockApplicationService;
    protected readonly ProductFieldMaskConfiguration Configuration;

    protected UpdateProductControllerTestBase()
    {
        MockApplicationService = Mock.Of<IProductApplicationService>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
        Configuration = new ProductFieldMaskConfiguration();
    }

    protected UpdateProductController UpdateProductController()
    {
        return new UpdateProductController(
            MockApplicationService,
            Configuration,
            Mapper);
    }
}
