// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using System.Net;
using backend.Product.ProductControllers;
using backend.Supplier.ApplicationLayer.Queries.GetSupplierView;
using backend.Supplier.DomainModels;
using backend.Supplier.SupplierControllers;
using backend.Supplier.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Supplier.Tests.ControllerLayerTests;

public class GetSupplierControllerTests : GetSupplierControllerTestBase
{
    [Fact]
    public async Task GetSupplier_ReturnsOkResult_WhenSupplierExists()
    {
        long supplierId = Fixture.Create<long>();
        var supplierView = new SupplierViewTestDataBuilder()
            .WithId(supplierId)
            .WithFullName("John Doe")
            .WithEmail("john.doe@example.com")
            .Build();
        var request = Fixture.Build<GetSupplierRequest>()
            .With(r => r.FieldMask, ["FullName", "Email"])
            .Create();
        Mock
            .Get(MockGetSupplierView)
            .Setup(service => service.Handle(It.Is<GetSupplierViewQuery>(q => q.Id == supplierId)))
            .ReturnsAsync(supplierView);
        GetSupplierController sut = GetSupplierController();

        ActionResult<GetProductResponse> result = await sut.GetSupplier(supplierId, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("John Doe");
        jsonResult.ShouldContain("john.doe@example.com");
    }

    [Fact]
    public async Task GetSupplier_ReturnsNotFound_WhenSupplierDoesNotExist()
    {
        long supplierId = Fixture.Create<long>();
        var request = Fixture.Create<GetSupplierRequest>();
        Mock
            .Get(MockGetSupplierView)
            .Setup(service => service.Handle(It.Is<GetSupplierViewQuery>(q => q.Id == supplierId)))
            .ReturnsAsync((SupplierView?)null);
        GetSupplierController sut = GetSupplierController();

        ActionResult<GetProductResponse> result = await sut.GetSupplier(supplierId, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetSupplier_SerializesWithFieldMaskCorrectly()
    {
        long supplierId = Fixture.Create<long>();
        var supplierView = new SupplierViewTestDataBuilder()
            .WithId(supplierId)
            .WithFullName("John Doe")
            .WithEmail("john.doe@example.com")
            .Build();
        var request = Fixture.Build<GetSupplierRequest>()
            .With(r => r.FieldMask, ["FullName"])
            .Create();
        Mock
            .Get(MockGetSupplierView)
            .Setup(service => service.Handle(It.Is<GetSupplierViewQuery>(q => q.Id == supplierId)))
            .ReturnsAsync(supplierView);
        GetSupplierController sut = GetSupplierController();

        ActionResult<GetProductResponse> result = await sut.GetSupplier(supplierId, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("John Doe");
        jsonResult.ShouldNotContain("john.doe@example.com");
    }
}
