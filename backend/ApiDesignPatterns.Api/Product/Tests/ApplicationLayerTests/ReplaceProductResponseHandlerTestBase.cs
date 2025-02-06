// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Queries.ReplaceProductResponse;
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

    protected IQueryHandler<ReplaceProductResponseQuery, ProductControllers.ReplaceProductResponse>
        GetReplaceProductResponseHandler()
    {
        return new ReplaceProductResponseHandler(Mapper);
    }
}
