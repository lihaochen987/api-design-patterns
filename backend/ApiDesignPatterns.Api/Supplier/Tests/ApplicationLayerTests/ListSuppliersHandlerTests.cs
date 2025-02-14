// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.DomainModels;
using backend.Supplier.SupplierControllers;
using Shouldly;
using Xunit;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public class ListSuppliersHandlerTests : ListSuppliersHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnSuppliersAndNextPageToken()
    {
        var request = new ListSuppliersRequest { Filter = "Email.EndsWith(@example.com)", MaxPageSize = 5 };
        Repository.AddSupplierView("John", "Doe", "john@example.com");
        Repository.AddSupplierView("Jane", "Smith", "jane@example.com");
        IQueryHandler<ListSuppliersQuery, (List<SupplierView>, string?)> sut = ListSuppliersViewHandler();

        (List<SupplierView>, string?) result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result.Item1.ShouldNotBeEmpty();
        result.Item1.Count.ShouldBe(2);
        result.Item2.ShouldBeNull();
        result.Item1.ShouldContain(s => s.Email == "john@example.com");
        result.Item1.ShouldContain(s => s.Email == "jane@example.com");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoSuppliersExist()
    {
        var request = new ListSuppliersRequest();
        IQueryHandler<ListSuppliersQuery, (List<SupplierView>, string?)> sut = ListSuppliersViewHandler();

        (List<SupplierView>, string?) result = await sut.Handle(
            new ListSuppliersQuery { Request = request });

        result.Item1.ShouldBeEmpty();
        result.Item2.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_ShouldFilterByName()
    {
        var request = new ListSuppliersRequest { Filter = "FullName == \"John Doe\"", MaxPageSize = 5 };
        Repository.AddSupplierView("John", "Doe", "john@example.com");
        Repository.AddSupplierView("Jane", "Smith", "jane@example.com");
        IQueryHandler<ListSuppliersQuery, (List<SupplierView>, string?)> sut = ListSuppliersViewHandler();

        (List<SupplierView>, string?) result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result.Item1.Count.ShouldBe(1);
        result.Item1.Single().FullName.ShouldBe("John Doe");
        result.Item2.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_ShouldRespectPageSize()
    {
        var request = new ListSuppliersRequest { MaxPageSize = 2 };
        Repository.AddSupplierView("John", "Doe", "john@example.com");
        Repository.AddSupplierView("Jane", "Smith", "jane@example.com");
        Repository.AddSupplierView("Bob", "Johnson", "bob@example.com");
        IQueryHandler<ListSuppliersQuery, (List<SupplierView>, string?)> sut = ListSuppliersViewHandler();

        (List<SupplierView>, string?) result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result.Item1.Count.ShouldBe(2);
        result.Item2.ShouldNotBeNull();
    }
}
