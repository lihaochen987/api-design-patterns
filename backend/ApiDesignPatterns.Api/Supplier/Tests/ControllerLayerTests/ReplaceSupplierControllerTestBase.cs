// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Commands.ReplaceSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.Controllers;
using backend.Supplier.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Supplier.Tests.ControllerLayerTests;

public abstract class ReplaceSupplierControllerTestBase
{
    protected readonly IAsyncQueryHandler<GetSupplierQuery, DomainModels.Supplier?> GetSupplier =
        Mock.Of<IAsyncQueryHandler<GetSupplierQuery, DomainModels.Supplier?>>();

    protected readonly ICommandHandler<ReplaceSupplierCommand> ReplaceSupplier =
        Mock.Of<ICommandHandler<ReplaceSupplierCommand>>();

    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected ReplaceSupplierControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterSupplierMappings();
        Mapper = new Mapper(config);
    }

    protected ReplaceSupplierController GetReplaceSupplierController()
    {
        return new ReplaceSupplierController(
            GetSupplier,
            ReplaceSupplier,
            Mapper);
    }
}
