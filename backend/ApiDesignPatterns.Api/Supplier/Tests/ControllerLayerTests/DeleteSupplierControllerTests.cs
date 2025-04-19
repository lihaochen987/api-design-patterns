// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.ApplicationLayer.Commands.DeleteSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.Controllers;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Supplier.Tests.ControllerLayerTests;

public class DeleteSupplierControllerTests : DeleteSupplierControllerTestBase
{
    [Fact]
    public async Task DeleteSupplier_SupplierExists_ReturnsNoContent()
    {
        DomainModels.Supplier supplier = new SupplierTestDataBuilder().Build();
        Mock
            .Get(MockGetSupplierHandler)
            .Setup(svc => svc.Handle(It.Is<GetSupplierQuery>(q => q.Id == supplier.Id)))
            .ReturnsAsync(supplier);
        DeleteSupplierController sut = DeleteSupplierController();

        ActionResult result = await sut.DeleteSupplier(supplier.Id, new DeleteSupplierRequest());

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteSupplier_SupplierDoesNotExist_ReturnsNotFound()
    {
        DomainModels.Supplier supplier = new SupplierTestDataBuilder().Build();
        Mock
            .Get(MockGetSupplierHandler)
            .Setup(svc => svc.Handle(It.Is<GetSupplierQuery>(q => q.Id == supplier.Id)))
            .ReturnsAsync((DomainModels.Supplier?)null);
        DeleteSupplierController sut = DeleteSupplierController();

        ActionResult result = await sut.DeleteSupplier(supplier.Id, new DeleteSupplierRequest());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteSupplier_HandlesDeleteFailure_ThrowsException()
    {
        DomainModels.Supplier supplier = new SupplierTestDataBuilder().Build();
        Mock
            .Get(MockGetSupplierHandler)
            .Setup(svc => svc.Handle(It.Is<GetSupplierQuery>(q => q.Id == supplier.Id)))
            .ReturnsAsync(supplier);
        Mock
            .Get(MockDeleteSupplierHandler)
            .Setup(svc => svc.Handle(It.IsAny<DeleteSupplierCommand>()))
            .ThrowsAsync(new Exception("Failed to delete supplier"));
        DeleteSupplierController sut = DeleteSupplierController();

        Func<Task> act = async () => await sut.DeleteSupplier(supplier.Id, new DeleteSupplierRequest());

        await act.Should().ThrowAsync<Exception>();
    }
}
