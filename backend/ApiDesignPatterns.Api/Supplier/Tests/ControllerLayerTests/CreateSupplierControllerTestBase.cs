// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.CreateSupplier;
using backend.Supplier.Services;
using backend.Supplier.SupplierControllers;
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
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<SupplierMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected CreateSupplierController GetCreateSupplierController()
    {
        return new CreateSupplierController(
            CreateSupplier,
            Mapper);
    }
}
