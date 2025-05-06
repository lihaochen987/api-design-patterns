// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class MapCreateProductRequestHandlerTestBase
{
    protected readonly IMapper Mapper;

    protected MapCreateProductRequestHandlerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterProductMappings();
        Mapper = new Mapper(config);
    }

    protected ISyncQueryHandler<MapCreateProductRequestQuery, DomainModels.Product> GetMapCreateProductRequestHandler()
    {
        return new MapCreateProductRequestHandler(Mapper);
    }
}
