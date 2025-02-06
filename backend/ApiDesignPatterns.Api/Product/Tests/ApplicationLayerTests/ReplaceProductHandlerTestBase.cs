// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.Services;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class ReplaceProductHandlerTestBase
{
    protected readonly ProductRepositoryFake Repository = [];
    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected ReplaceProductHandlerTestBase()
    {
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfig.CreateMapper();
    }

    protected ICommandHandler<ReplaceProductCommand> ReplaceProductHandler()
    {
        return new ReplaceProductHandler(Repository, Mapper);
    }
}
