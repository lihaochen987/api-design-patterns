// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Product.Controllers.Product;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class MapCreateProductResponseHandlerTestBase
{
    protected readonly IMapper Mapper;

    protected MapCreateProductResponseHandlerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected IQueryHandler<MapCreateProductResponseQuery, CreateProductResponse> GetCreateProductResponseHandler()
    {
        return new MapCreateProductResponseHandler(Mapper);
    }
}
