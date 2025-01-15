// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ProductControllers;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class ListProductsControllerTestBase
{
    protected readonly IProductViewQueryApplicationService MockQueryApplicationService;
    private readonly IMapper _mapper;
    protected const int DefaultMaxPageSize = 10;

    protected ListProductsControllerTestBase()
    {
        MockQueryApplicationService = Mock.Of<IProductViewQueryApplicationService>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
    }

    protected ListProductsController ListProductsController()
    {
        return new ListProductsController(MockQueryApplicationService, _mapper);
    }
}
