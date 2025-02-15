// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.DomainModels;
using backend.Supplier.Services;
using backend.Supplier.SupplierControllers;
using Moq;

namespace backend.Supplier.Tests.ControllerLayerTests;

public abstract class ListSuppliersControllerTestBase
{
    protected readonly IQueryHandler<ListSuppliersQuery, (List<SupplierView>, string?)> MockListSuppliers;
    private readonly IMapper _mapper;
    protected const int DefaultMaxPageSize = 10;
    protected Fixture Fixture = new();

    protected ListSuppliersControllerTestBase()
    {
        MockListSuppliers = Mock.Of<IQueryHandler<ListSuppliersQuery, (List<SupplierView>, string?)>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<SupplierMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
    }

    protected ListSuppliersController ListSuppliersController()
    {
        return new ListSuppliersController(MockListSuppliers, _mapper);
    }
}
