// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class MapCreateProductRequestHandlerTestBase
{
    protected readonly IMapper Mapper;

    protected MapCreateProductRequestHandlerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected ISyncQueryHandler<MapCreateProductRequestQuery, DomainModels.Product> GetMapCreateProductRequestHandler()
    {
        return new MapCreateProductRequestHandler(Mapper);
    }
}
