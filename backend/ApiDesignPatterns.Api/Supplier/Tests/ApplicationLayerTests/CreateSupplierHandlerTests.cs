// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.CreateSupplier;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public class CreateSupplierHandlerTests : CreateSupplierHandlerTestBase
{
    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectSupplier()
    {
        var supplierToCreate = new SupplierTestDataBuilder().Build();
        var command = new CreateSupplierCommand { Supplier = supplierToCreate };
        ICommandHandler<CreateSupplierCommand> sut = CreateSupplierService();

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        var createdSupplier = Repository.First();
        createdSupplier.Id.Should().NotBe(0);
        createdSupplier.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        createdSupplier.Should().BeEquivalentTo(supplierToCreate,
            options => options.Excluding(s => s.CreatedAt));
    }

    [Fact]
    public async Task Handle_PersistsWhenCalledTwice()
    {
        var firstSupplierToCreate = new SupplierTestDataBuilder().Build();
        var secondSupplierToCreate = new SupplierTestDataBuilder().Build();
        var firstCommand = new CreateSupplierCommand { Supplier = firstSupplierToCreate };
        var secondCommand = new CreateSupplierCommand { Supplier = secondSupplierToCreate };
        ICommandHandler<CreateSupplierCommand> sut = CreateSupplierService();

        await sut.Handle(firstCommand);
        await sut.Handle(secondCommand);

        Repository.IsDirty.Should().BeTrue();

        var firstSupplier = Repository.First(x => x.Id == firstSupplierToCreate.Id);
        firstSupplier.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        firstSupplier.Should().BeEquivalentTo(firstSupplierToCreate,
            options => options.Excluding(s => s.CreatedAt));

        var secondSupplier = Repository.First(x => x.Id == secondSupplierToCreate.Id);
        secondSupplier.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        secondSupplier.Should().BeEquivalentTo(secondSupplierToCreate,
            options => options.Excluding(s => s.CreatedAt));
    }
}
