// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.ApplicationLayer.Commands.CreateSupplier;
using backend.Supplier.SupplierControllers;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
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

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<CreateSupplierResponse>();
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

        Func<Task> act = async () => await sut.CreateSupplier(request);

        await act.Should().ThrowAsync<Exception>();
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

        var response = result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<CreateSupplierResponse>().Subject;
        response.FirstName.Should().Be(firstName);
        response.LastName.Should().Be(lastName);
        response.Email.Should().Be(email);
    }
}
