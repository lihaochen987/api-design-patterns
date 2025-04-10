// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.Controllers;
using FluentAssertions;
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

        PagedSuppliers result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result.Suppliers.Should().NotBeEmpty();
        result.Suppliers.Should().HaveCount(2);
        result.NextPageToken.Should().BeNull();
        result.Suppliers.Should().Contain(s => s.Email == "john@example.com");
        result.Suppliers.Should().Contain(s => s.Email == "jane@example.com");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoSuppliersExist()
    {
        var request = new ListSuppliersRequest();
        IQueryHandler<ListSuppliersQuery, PagedSuppliers> sut = ListSuppliersViewHandler();

        PagedSuppliers result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result.Suppliers.Should().BeEmpty();
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldFilterByName()
    {
        var request = new ListSuppliersRequest { Filter = "FullName == \"John Doe\"", MaxPageSize = 5 };
        Repository.AddSupplierView("John", "Doe", "john@example.com");
        Repository.AddSupplierView("Jane", "Smith", "jane@example.com");
        IQueryHandler<ListSuppliersQuery, PagedSuppliers> sut = ListSuppliersViewHandler();

        PagedSuppliers result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result.Suppliers.Should().HaveCount(1);
        result.Suppliers.Single().FullName.Should().Be("John Doe");
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldRespectPageSize()
    {
        var request = new ListSuppliersRequest { MaxPageSize = 2 };
        Repository.AddSupplierView("John", "Doe", "john@example.com");
        Repository.AddSupplierView("Jane", "Smith", "jane@example.com");
        Repository.AddSupplierView("Bob", "Johnson", "bob@example.com");
        IQueryHandler<ListSuppliersQuery, PagedSuppliers> sut = ListSuppliersViewHandler();

        PagedSuppliers result = await sut.Handle(new ListSuppliersQuery { Request = request });

        result.Suppliers.Should().HaveCount(2);
        result.NextPageToken.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenRepositoryFails()
    {
        var request = new ListSuppliersRequest { Filter = "InvalidFilter == \"SomeValue\"", MaxPageSize = 5 };
        IQueryHandler<ListSuppliersQuery, PagedSuppliers> sut = ListSuppliersViewHandler();

        Func<Task> act = async () => await sut.Handle(new ListSuppliersQuery { Request = request });

        await act.Should().ThrowAsync<ArgumentException>();
    }
}
