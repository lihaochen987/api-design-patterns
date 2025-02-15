// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.Services;
using backend.Supplier.SupplierControllers;
using Moq;

namespace backend.Supplier.Tests.ControllerLayerTests;

public abstract class UpdateSupplierControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly IQueryHandler<GetSupplierQuery, DomainModels.Supplier> MockGetSupplierHandler;
    protected readonly ICommandHandler<UpdateSupplierCommand> MockUpdateSupplierHandler;
    protected readonly Fixture Fixture = new();

    protected UpdateSupplierControllerTestBase()
    {
        MockGetSupplierHandler = Mock.Of<IQueryHandler<GetSupplierQuery, DomainModels.Supplier>>();
        MockUpdateSupplierHandler = Mock.Of<ICommandHandler<UpdateSupplierCommand>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<SupplierMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected UpdateSupplierController UpdateSupplierController()
    {
        return new UpdateSupplierController(
            MockGetSupplierHandler,
            MockUpdateSupplierHandler,
            Mapper);
    }
}
