// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.MapReplaceProductResponse;
using backend.Product.Controllers.Product;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class MapReplaceProductResponseHandlerTestBase
{
    protected readonly IMapper Mapper;

    protected MapReplaceProductResponseHandlerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterProductMappings();
        Mapper = new Mapper(config);
    }

    protected ISyncQueryHandler<MapReplaceProductResponseQuery, ReplaceProductResponse>
        GetReplaceProductResponseHandler()
    {
        return new MapReplaceProductResponseHandler(Mapper);
    }
}
