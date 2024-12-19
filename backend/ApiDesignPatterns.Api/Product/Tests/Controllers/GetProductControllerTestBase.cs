// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ProductControllers;
using backend.Product.Services;
using Moq;

namespace backend.Product.Tests.Controllers;

public abstract class GetProductControllerTestBase
{
    protected readonly Fixture Fixture;
    protected readonly IProductViewApplicationService MockApplicationService;
    private readonly IMapper _mapper;
    private readonly ProductFieldMaskConfiguration _configuration;

    protected GetProductControllerTestBase()
    {
        Fixture = new Fixture();
        MockApplicationService = Mock.Of<IProductViewApplicationService>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<GetProductMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
        _configuration = new ProductFieldMaskConfiguration();
    }

    protected GetProductController GetProductController()
    {
        return new GetProductController(MockApplicationService, _configuration, _mapper);
    }
}
