// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.Controllers;
using backend.Supplier.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Supplier.Tests.ControllerLayerTests;

public abstract class UpdateSupplierControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly IAsyncQueryHandler<GetSupplierQuery, DomainModels.Supplier?> MockGetSupplierHandler;
    protected readonly ICommandHandler<UpdateSupplierCommand> MockUpdateSupplierHandler;
    protected readonly Fixture Fixture = new();

    protected UpdateSupplierControllerTestBase()
    {
        MockGetSupplierHandler = Mock.Of<IAsyncQueryHandler<GetSupplierQuery, DomainModels.Supplier?>>();
        MockUpdateSupplierHandler = Mock.Of<ICommandHandler<UpdateSupplierCommand>>();
        var config = new TypeAdapterConfig();
        config.RegisterSupplierMappings();
        Mapper = new Mapper(config);
    }

    protected UpdateSupplierController UpdateSupplierController()
    {
        return new UpdateSupplierController(
            MockGetSupplierHandler,
            MockUpdateSupplierHandler,
            Mapper);
    }
}
