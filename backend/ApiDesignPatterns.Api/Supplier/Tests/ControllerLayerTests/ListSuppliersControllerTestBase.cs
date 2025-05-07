// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.Controllers;
using backend.Supplier.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Supplier.Tests.ControllerLayerTests;

public abstract class ListSuppliersControllerTestBase
{
    protected readonly IAsyncQueryHandler<ListSuppliersQuery, PagedSuppliers> MockListSuppliers;
    protected readonly IMapper Mapper;
    protected const int DefaultMaxPageSize = 10;

    protected ListSuppliersControllerTestBase()
    {
        MockListSuppliers = Mock.Of<IAsyncQueryHandler<ListSuppliersQuery, PagedSuppliers>>();
        var config = new TypeAdapterConfig();
        config.RegisterSupplierMappings();
        Mapper = new Mapper(config);
    }

    protected ListSuppliersController ListSuppliersController()
    {
        return new ListSuppliersController(MockListSuppliers, Mapper);
    }
}
