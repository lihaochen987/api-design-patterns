// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.SupplierControllers;
using backend.Supplier.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Supplier.Tests.ControllerLayerTests;

public class UpdateSupplierControllerTests : UpdateSupplierControllerTestBase
{
    [Fact]
    public async Task UpdateSupplier_WithValidRequest_ShouldReturnUpdatedSupplier()
    {
        var supplier = new SupplierTestDataBuilder().Build();
        UpdateSupplierRequest request = new() { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        Mock
            .Get(MockGetSupplierHandler)
            .Setup(svc => svc.Handle(It.Is<GetSupplierQuery>(q => q.Id == supplier.Id)))
            .ReturnsAsync(supplier);
        UpdateSupplierController sut = UpdateSupplierController();

        ActionResult<UpdateSupplierResponse> actionResult = await sut.UpdateSupplier(supplier.Id, request);

        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        UpdateSupplierResponse? response = contentResult!.Value as UpdateSupplierResponse;
        response.ShouldBeEquivalentTo(Mapper.Map<UpdateSupplierResponse>(supplier));
        Mock
            .Get(MockUpdateSupplierHandler)
            .Verify(
                svc => svc.Handle(It.IsAny<UpdateSupplierCommand>()),
                Times.Once);
    }

    [Fact]
    public async Task UpdateSupplier_NonExistentSupplier_ShouldReturnNotFound()
    {
        UpdateSupplierRequest request = new() { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        long nonExistentId = Fixture.Create<long>();
        Mock
            .Get(MockGetSupplierHandler)
            .Setup(svc => svc.Handle(It.Is<GetSupplierQuery>(q => q.Id == nonExistentId)))
            .ReturnsAsync((DomainModels.Supplier?)null);
        var sut = UpdateSupplierController();

        ActionResult<UpdateSupplierResponse> actionResult = await sut.UpdateSupplier(nonExistentId, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<NotFoundResult>();
        Mock
            .Get(MockUpdateSupplierHandler)
            .Verify(
                svc => svc.Handle(It.IsAny<UpdateSupplierCommand>()),
                Times.Never);
    }

    [Fact]
    public async Task UpdateSupplier_WhenUpdateSucceeds_ShouldReturnMappedResponse()
    {
        var supplier = new SupplierTestDataBuilder().Build();
        UpdateSupplierRequest request = new() { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        Mock
            .Get(MockGetSupplierHandler)
            .Setup(svc => svc.Handle(It.Is<GetSupplierQuery>(q => q.Id == supplier.Id)))
            .ReturnsAsync(supplier);
        var expectedResponse = Mapper.Map<UpdateSupplierResponse>(supplier);
        var sut = UpdateSupplierController();

        var result = await sut.UpdateSupplier(supplier.Id, request);

        result.ShouldNotBeNull();
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task UpdateSupplier_VerifiesSecondGetCall_AfterUpdate()
    {
        var supplier = new SupplierTestDataBuilder().Build();
        var updatedSupplier = new SupplierTestDataBuilder()
            .WithId(supplier.Id)
            .WithFirstName("Updated")
            .Build();
        UpdateSupplierRequest request = new() { FirstName = "Updated" };
        Mock
            .Get(MockGetSupplierHandler)
            .SetupSequence(svc => svc.Handle(It.Is<GetSupplierQuery>(q => q.Id == supplier.Id)))
            .ReturnsAsync(supplier)
            .ReturnsAsync(updatedSupplier);
        var sut = UpdateSupplierController();

        var result = await sut.UpdateSupplier(supplier.Id, request);

        Mock
            .Get(MockGetSupplierHandler)
            .Verify(
                svc => svc.Handle(It.Is<GetSupplierQuery>(q => q.Id == supplier.Id)),
                Times.Exactly(2));
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        var response = okResult.Value as UpdateSupplierResponse;
        response.ShouldNotBeNull();
        response.FirstName.ShouldBe("Updated");
    }

    [Fact]
    public async Task UpdateSupplier_PassesCorrectCommandParameters()
    {
        var supplier = new SupplierTestDataBuilder().Build();
        UpdateSupplierRequest request = new() { FirstName = "John", Email = "john@example.com" };
        Mock
            .Get(MockGetSupplierHandler)
            .Setup(svc => svc.Handle(It.Is<GetSupplierQuery>(q => q.Id == supplier.Id)))
            .ReturnsAsync(supplier);
        var sut = UpdateSupplierController();

        await sut.UpdateSupplier(supplier.Id, request);

        Mock
            .Get(MockUpdateSupplierHandler)
            .Verify(
                svc => svc.Handle(It.Is<UpdateSupplierCommand>(cmd =>
                    cmd.Request == request &&
                    cmd.Supplier == supplier)),
                Times.Once);
    }
}
