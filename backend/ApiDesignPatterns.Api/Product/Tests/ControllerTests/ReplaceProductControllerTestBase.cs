// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ApplicationLayer.Queries.MapReplaceProductResponse;
using backend.Product.Controllers.Product;
using backend.Product.Services.Mappers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class ReplaceProductControllerTestBase
{
    protected readonly IQueryHandler<GetProductQuery, DomainModels.Product?> GetProduct =
        Mock.Of<IQueryHandler<GetProductQuery, DomainModels.Product?>>();

    protected readonly ICommandHandler<ReplaceProductCommand> ReplaceProduct =
        Mock.Of<ICommandHandler<ReplaceProductCommand>>();

    protected readonly IQueryHandler<MapReplaceProductResponseQuery, ReplaceProductResponse> MapReplaceProductResponse;

    protected readonly IMapper Mapper;

    protected ReplaceProductControllerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
        MapReplaceProductResponse = new MapReplaceProductResponseHandler(Mapper);
    }

    protected ReplaceProductController GetReplaceProductController()
    {
        return new ReplaceProductController(
            GetProduct,
            ReplaceProduct,
            MapReplaceProductResponse);
    }
}
