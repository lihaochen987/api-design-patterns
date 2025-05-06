// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.Services.Mappers;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;
using Mapster;
using MapsterMapper;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class ReplaceProductHandlerTestBase
{
    protected readonly ProductRepositoryFake Repository = [];
    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected ReplaceProductHandlerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterProductMappings();
        Mapper = new Mapper(config);
    }

    protected ICommandHandler<ReplaceProductCommand> ReplaceProductHandler()
    {
        return new ReplaceProductHandler(Repository, Mapper);
    }
}
