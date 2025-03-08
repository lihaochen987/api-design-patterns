// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Queries.MapListProductsResponse;
using backend.Product.ProductControllers;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class MapListProductsResponseHandlerTestBase
{
    protected readonly IMapper Mapper;

    protected MapListProductsResponseHandlerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected IQueryHandler<MapListProductsResponseQuery, ListProductsResponse>
        GetListProductsResponseHandler()
    {
        return new MapListProductsResponseHandler(Mapper);
    }
}
