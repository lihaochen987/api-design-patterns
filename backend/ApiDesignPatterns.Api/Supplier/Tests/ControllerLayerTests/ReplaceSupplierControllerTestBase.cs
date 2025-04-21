// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Commands.ReplaceSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.Controllers;
using backend.Supplier.Services;
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
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<SupplierMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected ReplaceSupplierController GetReplaceSupplierController()
    {
        return new ReplaceSupplierController(
            GetSupplier,
            ReplaceSupplier,
            Mapper);
    }
}
