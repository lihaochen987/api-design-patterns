// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.ApplicationLayer.Commands.ReplaceSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.SupplierControllers;
using backend.Supplier.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Supplier.Tests.ControllerLayerTests;

public class ReplaceSupplierControllerTests : ReplaceSupplierControllerTestBase
{
    [Fact]
    public async Task ReplaceSupplier_ReturnsOkResponse_WhenSupplierReplacedSuccessfully()
    {
        var supplier = new SupplierTestDataBuilder().Build();
        var request = Mapper.Map<ReplaceSupplierRequest>(supplier);
        Mock
            .Get(GetSupplier)
            .Setup(x => x.Handle(It.Is<GetSupplierQuery>(q => q.Id == supplier.Id)))
            .ReturnsAsync(supplier);
        ReplaceSupplierController sut = GetReplaceSupplierController();

        await sut.ReplaceSupplier(supplier.Id, request);

        Mock
            .Get(ReplaceSupplier)
            .Verify(x => x.Handle(It.IsAny<ReplaceSupplierCommand>()), Times.Once);
    }

    [Fact]
    public async Task ReplaceSupplier_ReturnsNotFound_WhenSupplierDoesNotExist()
    {
        long nonExistentId = Fixture.Create<long>();
        var supplier = new SupplierTestDataBuilder().Build();
        var request = Mapper.Map<ReplaceSupplierRequest>(supplier);
        Mock
            .Get(GetSupplier)
            .Setup(x => x.Handle(It.Is<GetSupplierQuery>(q => q.Id == nonExistentId)))
            .ReturnsAsync((DomainModels.Supplier?)null);
        ReplaceSupplierController sut = GetReplaceSupplierController();

        var result = await sut.ReplaceSupplier(nonExistentId, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
        Mock
            .Get(ReplaceSupplier)
            .Verify(x => x.Handle(It.IsAny<ReplaceSupplierCommand>()), Times.Never);
    }

    [Fact]
    public async Task ReplaceSupplier_HandlesMappingAndCommand_WithCorrectData()
    {
        var supplier = new SupplierTestDataBuilder()
            .WithFirstName("John")
            .WithLastName("Doe")
            .WithEmail("john.doe@example.com")
            .Build();
        var request = Mapper.Map<ReplaceSupplierRequest>(supplier);
        Mock
            .Get(GetSupplier)
            .Setup(x => x.Handle(It.Is<GetSupplierQuery>(q => q.Id == supplier.Id)))
            .ReturnsAsync(supplier);
        ReplaceSupplierController sut = GetReplaceSupplierController();

        var result = await sut.ReplaceSupplier(supplier.Id, request);

        result.ShouldNotBeNull();
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (ReplaceSupplierResponse)okResult.Value!;
        response.ShouldNotBeNull();
        response.FirstName.ShouldBe("John");
        response.LastName.ShouldBe("Doe");
        response.Email.ShouldBe("john.doe@example.com");
        Mock
            .Get(ReplaceSupplier)
            .Verify(x => x.Handle(It.Is<ReplaceSupplierCommand>(c =>
                    c.Supplier.FirstName == "John" &&
                    c.Supplier.LastName == "Doe" &&
                    c.Supplier.Email == "john.doe@example.com")),
                Times.Once);
    }
}
