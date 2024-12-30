// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ProductControllers;
using backend.Product.Services.ProductServices;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class GetProductControllerTestBase
{
    protected readonly Fixture Fixture;
    protected readonly IProductViewApplicationService MockApplicationService;
    private readonly IMapper _mapper;
    private readonly ProductFieldPaths _productFieldPaths;

    protected GetProductControllerTestBase()
    {
        Fixture = new Fixture();
        MockApplicationService = Mock.Of<IProductViewApplicationService>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
        _productFieldPaths = new ProductFieldPaths();
    }

    protected GetProductController GetProductController()
    {
        return new GetProductController(MockApplicationService, _productFieldPaths, _mapper);
    }
}
