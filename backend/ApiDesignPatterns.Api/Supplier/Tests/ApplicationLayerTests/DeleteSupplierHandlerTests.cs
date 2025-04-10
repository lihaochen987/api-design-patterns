// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.DeleteSupplier;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public class DeleteSupplierHandlerTests : DeleteSupplierHandlerTestBase
{
    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectSupplier()
    {
        DomainModels.Supplier supplierToDelete = new SupplierTestDataBuilder().Build();
        Repository.Add(supplierToDelete);
        ICommandHandler<DeleteSupplierCommand> sut = DeleteSupplierService();

        await sut.Handle(new DeleteSupplierCommand { Id = supplierToDelete.Id });

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_DoesNotThrowException_WhenSupplierDoesNotExist()
    {
        long nonExistentId = Fixture.Create<long>();
        ICommandHandler<DeleteSupplierCommand> sut = DeleteSupplierService();

        await sut.Handle(new DeleteSupplierCommand { Id = nonExistentId });

        Repository.IsDirty.Should().BeFalse();
    }
}
