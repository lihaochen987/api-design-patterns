// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.ApplicationLayer.Commands.CreateSupplier;
using backend.Supplier.SupplierControllers;
using backend.Supplier.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Supplier.Tests.ControllerLayerTests;

public class CreateSupplierControllerTests : CreateSupplierControllerTestBase
{
    [Fact]
    public async Task CreateSupplier_ReturnsOkResponse_WhenSupplierCreatedSuccessfully()
    {
        var supplier = new SupplierTestDataBuilder().Build();
        var request = Mapper.Map<CreateSupplierRequest>(supplier);
        CreateSupplierController sut = GetCreateSupplierController();

        var result = await sut.CreateSupplier(request);

        result.ShouldNotBeNull();
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBeOfType<CreateSupplierResponse>();
        Mock
            .Get(CreateSupplier)
            .Verify(x => x.Handle(It.IsAny<CreateSupplierCommand>()),
                Times.Once);
    }

    [Fact]
    public async Task CreateSupplier_HandlesCommandFailure_WhenCreateSupplierFails()
    {
        var supplier = new SupplierTestDataBuilder().Build();
        var request = Mapper.Map<CreateSupplierRequest>(supplier);
        Mock
            .Get(CreateSupplier)
            .Setup(x => x.Handle(It.IsAny<CreateSupplierCommand>()))
            .ThrowsAsync(new Exception("Failed to create supplier"));
        var sut = GetCreateSupplierController();

        await Should.ThrowAsync<Exception>(() => sut.CreateSupplier(request));
    }

    [Theory]
    [InlineData("John", "Doe", "john.doe@example.com")]
    [InlineData("Jane", "Smith", "jane.smith@example.com")]
    public async Task CreateSupplier_ValidatesMapping_WithDifferentSupplierData(
        string firstName, string lastName, string email)
    {
        var supplier = new SupplierTestDataBuilder()
            .WithFirstName(firstName)
            .WithLastName(lastName)
            .WithEmail(email)
            .Build();
        var request = Mapper.Map<CreateSupplierRequest>(supplier);
        CreateSupplierController sut = GetCreateSupplierController();

        var result = await sut.CreateSupplier(request);

        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        var response = okResult.Value.ShouldBeOfType<CreateSupplierResponse>();
        response.FirstName.ShouldBe(firstName);
        response.LastName.ShouldBe(lastName);
        response.Email.ShouldBe(email);
    }
}
