// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.Controllers;
using backend.Supplier.Services;
using Moq;

namespace backend.Supplier.Tests.ControllerLayerTests;

public abstract class ListSuppliersControllerTestBase
{
    protected readonly IAsyncQueryHandler<ListSuppliersQuery, PagedSuppliers> MockListSuppliers;
    private readonly IMapper _mapper;
    protected const int DefaultMaxPageSize = 10;

    protected ListSuppliersControllerTestBase()
    {
        MockListSuppliers = Mock.Of<IAsyncQueryHandler<ListSuppliersQuery, PagedSuppliers>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<SupplierMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
    }

    protected ListSuppliersController ListSuppliersController()
    {
        return new ListSuppliersController(MockListSuppliers, _mapper);
    }
}
