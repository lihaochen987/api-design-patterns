// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.CreateSupplier;
using backend.Supplier.Controllers;
using backend.Supplier.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Supplier.Tests.ControllerLayerTests;

public abstract class CreateSupplierControllerTestBase
{
    protected readonly ICommandHandler<CreateSupplierCommand> CreateSupplier =
        Mock.Of<ICommandHandler<CreateSupplierCommand>>();

    protected readonly IMapper Mapper;

    protected Fixture Fixture = new();

    protected CreateSupplierControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterSupplierMappings();
        Mapper = new Mapper(config);
    }

    protected CreateSupplierController GetCreateSupplierController()
    {
        return new CreateSupplierController(
            CreateSupplier,
            Mapper);
    }
}
