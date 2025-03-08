// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Queries.MapReplaceProductResponse;
using backend.Product.Services;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class ReplaceProductResponseHandlerTestBase
{
    protected readonly IMapper Mapper;

    protected ReplaceProductResponseHandlerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected IQueryHandler<MapReplaceProductResponseQuery, ProductControllers.ReplaceProductResponse>
        GetReplaceProductResponseHandler()
    {
        return new MapReplaceProductResponseHandler(Mapper);
    }
}
