// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.Controllers;
using backend.Supplier.DomainModels;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Supplier.Tests.ControllerLayerTests;

public class ListSuppliersControllerTests : ListSuppliersControllerTestBase
{
    [Fact]
    public async Task ListSuppliers_ShouldReturnAllSuppliers_WhenNoPageTokenProvided()
    {
        List<SupplierView> supplierViews = new SupplierViewTestDataBuilder().CreateMany(4).ToList();
        ListSuppliersRequest request = new() { MaxPageSize = 4 };
        Mock
            .Get(MockListSuppliers)
            .Setup(svc => svc.Handle(It.Is<ListSuppliersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedSuppliers(supplierViews, null));
        ListSuppliersController sut = ListSuppliersController();

        var result = await sut.ListSuppliers(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        response.Should().NotBeNull();
        var listSuppliersResponse = response.Value.Should().BeOfType<ListSuppliersResponse>().Subject;
        listSuppliersResponse.Results.Should().HaveCount(4);
        listSuppliersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListSuppliers_ShouldReturnSuppliersAfterPageToken_WhenPageTokenProvided()
    {
        List<SupplierView> supplierViewList = new SupplierViewTestDataBuilder().CreateMany(4).ToList();
        var expectedPageResults = supplierViewList.Skip(2).Take(2).ToList();
        ListSuppliersRequest request = new() { PageToken = "2", MaxPageSize = 2 };
        Mock
            .Get(MockListSuppliers)
            .Setup(svc => svc.Handle(It.Is<ListSuppliersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedSuppliers(expectedPageResults, null));
        ListSuppliersController sut = ListSuppliersController();

        var result = await sut.ListSuppliers(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        response.Should().NotBeNull();
        var listSuppliersResponse = response.Value.Should().BeOfType<ListSuppliersResponse>().Subject;
        listSuppliersResponse.Results.Should().HaveCount(2);
        listSuppliersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListSuppliers_ShouldReturnNextPageToken_WhenMoreSuppliersExist()
    {
        List<SupplierView> suppliers = new SupplierViewTestDataBuilder().CreateMany(20).ToList();
        List<SupplierView> firstPageSuppliers = suppliers.Take(2).ToList();
        ListSuppliersRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListSuppliers)
            .Setup(svc => svc.Handle(It.Is<ListSuppliersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedSuppliers(firstPageSuppliers, "2"));
        ListSuppliersController sut = ListSuppliersController();

        var result = await sut.ListSuppliers(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        response.Should().NotBeNull();
        var listSuppliersResponse = response.Value.Should().BeOfType<ListSuppliersResponse>().Subject;
        listSuppliersResponse.Results.Should().HaveCount(2);
        listSuppliersResponse.NextPageToken.Should().BeEquivalentTo("2");
    }

    [Fact]
    public async Task ListSuppliers_ShouldUseDefaults_WhenPageTokenAndMaxPageSizeNotProvided()
    {
        List<SupplierView> suppliers = new SupplierViewTestDataBuilder().CreateMany(20).ToList();
        List<SupplierView> defaultPageSuppliers = suppliers.Take(DefaultMaxPageSize).ToList();
        ListSuppliersRequest request = new();
        Mock
            .Get(MockListSuppliers)
            .Setup(svc => svc.Handle(It.Is<ListSuppliersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedSuppliers(defaultPageSuppliers, DefaultMaxPageSize.ToString()));
        ListSuppliersController sut = ListSuppliersController();

        var result = await sut.ListSuppliers(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        response.Should().NotBeNull();
        var listSuppliersResponse = response.Value.Should().BeOfType<ListSuppliersResponse>().Subject;
        listSuppliersResponse.Results.Should().HaveCount(DefaultMaxPageSize);
        listSuppliersResponse.NextPageToken.Should().BeEquivalentTo(DefaultMaxPageSize.ToString());
    }

    [Fact]
    public async Task ListSuppliers_ShouldReturnEmptyList_WhenNoSuppliersExist()
    {
        ListSuppliersRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListSuppliers)
            .Setup(svc => svc.Handle(It.Is<ListSuppliersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedSuppliers([], null));
        ListSuppliersController sut = ListSuppliersController();

        var result = await sut.ListSuppliers(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        response.Should().NotBeNull();
        var listSuppliersResponse = response.Value.Should().BeOfType<ListSuppliersResponse>().Subject;
        listSuppliersResponse.Results.Should().BeEmpty();
        listSuppliersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListSuppliers_WithFilterAndPagination_ReturnsCorrectResults()
    {
        var supplier = new SupplierViewTestDataBuilder()
            .WithId(2)
            .WithFullName("John Doe")
            .Build();
        var filteredSuppliers = new List<SupplierView> { supplier };
        ListSuppliersRequest request = new() { Filter = "FullName == 'John Doe'", MaxPageSize = 2, PageToken = "1" };
        Mock
            .Get(MockListSuppliers)
            .Setup(svc => svc.Handle(It.Is<ListSuppliersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedSuppliers(filteredSuppliers, "2"));
        ListSuppliersController sut = ListSuppliersController();

        var result = await sut.ListSuppliers(request);

        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        var listSuppliersResponse = response.Value.Should().BeOfType<ListSuppliersResponse>().Subject;
        listSuppliersResponse.Results.Should().HaveCount(1);
        listSuppliersResponse.NextPageToken.Should().Be("2");
    }
}
