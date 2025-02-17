// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.DomainModels;
using backend.Supplier.SupplierControllers;
using backend.Supplier.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
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

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listSuppliersResponse = response.Value as ListSuppliersResponse;
        listSuppliersResponse!.Results.Count().ShouldBe(4);
        listSuppliersResponse.NextPageToken.ShouldBeNull();
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

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listSuppliersResponse = response.Value as ListSuppliersResponse;
        listSuppliersResponse!.Results.Count().ShouldBe(2);
        listSuppliersResponse.NextPageToken.ShouldBeNull();
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

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listSuppliersResponse = response.Value as ListSuppliersResponse;
        listSuppliersResponse!.Results.Count().ShouldBe(2);
        listSuppliersResponse.NextPageToken.ShouldBeEquivalentTo("2");
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

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listSuppliersResponse = response.Value as ListSuppliersResponse;
        listSuppliersResponse!.Results.Count().ShouldBe(DefaultMaxPageSize);
        listSuppliersResponse.NextPageToken.ShouldBeEquivalentTo(DefaultMaxPageSize.ToString());
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

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        response.ShouldNotBeNull();
        var listSuppliersResponse = response.Value as ListSuppliersResponse;
        listSuppliersResponse!.Results.ShouldBeEmpty();
        listSuppliersResponse.NextPageToken.ShouldBeNull();
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

        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = result.Result as OkObjectResult;
        var listSuppliersResponse = response!.Value as ListSuppliersResponse;
        listSuppliersResponse!.Results.Count().ShouldBe(1);
        listSuppliersResponse.NextPageToken.ShouldBe("2");
    }
}
