// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.Controllers;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
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

        var result = await sut.ReplaceSupplier(supplier.Id, request);

        result.Result.Should().BeOfType<OkObjectResult>();
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

        result.Result.Should().BeOfType<NotFoundResult>();
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

        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ReplaceSupplierResponse>().Subject;
        response.Should().NotBeNull();
        response.FirstName.Should().Be("John");
        response.LastName.Should().Be("Doe");
        response.Email.Should().Be("john.doe@example.com");
    }
}
