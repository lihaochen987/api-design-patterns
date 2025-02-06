// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Queries.CreateProductResponse;
using backend.Product.ProductControllers;
using backend.Product.Services;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class CreateProductResponseHandlerTestBase
{
    protected readonly IMapper Mapper;

    protected CreateProductResponseHandlerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected IQueryHandler<CreateProductResponseQuery, CreateProductResponse> GetCreateProductResponseHandler()
    {
        return new CreateProductResponseHandler(Mapper);
    }
}
