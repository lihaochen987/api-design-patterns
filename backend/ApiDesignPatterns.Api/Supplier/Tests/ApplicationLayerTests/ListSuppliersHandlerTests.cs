// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.SupplierControllers;
using Shouldly;
using Xunit;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public class ListSuppliersHandlerTests : ListSuppliersHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnSuppliersAndNextPageToken()
    {
        var request = new ListSuppliersRequest { Filter = "Email.endsWith(@example.com)", MaxPageSize = 5 };
        Repository.AddSupplierView("John", "Doe", "john@example.com");
        Repository.AddSupplierView("Jane", "Smith", "jane@example.com");
        IQueryHandler<ListSuppliersQuery, PagedSuppliers> sut = ListSuppliersViewHandler();

        PagedSuppliers? result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result!.Suppliers.ShouldNotBeEmpty();
        result.Suppliers.Count.ShouldBe(2);
        result.NextPageToken.ShouldBeNull();
        result.Suppliers.ShouldContain(s => s.Email == "john@example.com");
        result.Suppliers.ShouldContain(s => s.Email == "jane@example.com");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoSuppliersExist()
    {
        var request = new ListSuppliersRequest();
        IQueryHandler<ListSuppliersQuery, PagedSuppliers> sut = ListSuppliersViewHandler();

        PagedSuppliers? result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result!.Suppliers.ShouldBeEmpty();
        result.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_ShouldFilterByName()
    {
        var request = new ListSuppliersRequest { Filter = "FullName == \"John Doe\"", MaxPageSize = 5 };
        Repository.AddSupplierView("John", "Doe", "john@example.com");
        Repository.AddSupplierView("Jane", "Smith", "jane@example.com");
        IQueryHandler<ListSuppliersQuery, PagedSuppliers> sut = ListSuppliersViewHandler();

        PagedSuppliers? result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result!.Suppliers.Count.ShouldBe(1);
        result.Suppliers.Single().FullName.ShouldBe("John Doe");
        result.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_ShouldRespectPageSize()
    {
        var request = new ListSuppliersRequest { MaxPageSize = 2 };
        Repository.AddSupplierView("John", "Doe", "john@example.com");
        Repository.AddSupplierView("Jane", "Smith", "jane@example.com");
        Repository.AddSupplierView("Bob", "Johnson", "bob@example.com");
        IQueryHandler<ListSuppliersQuery, PagedSuppliers> sut = ListSuppliersViewHandler();

        PagedSuppliers? result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result!.Suppliers.Count.ShouldBe(2);
        result.NextPageToken.ShouldNotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenRepositoryFails()
    {
        var request = new ListSuppliersRequest { Filter = "InvalidFilter == \"SomeValue\"", MaxPageSize = 5 };
        IQueryHandler<ListSuppliersQuery, PagedSuppliers> sut = ListSuppliersViewHandler();

        await Should.ThrowAsync<ArgumentException>(() => sut.Handle(
            new ListSuppliersQuery { Request = request }));
    }
}
