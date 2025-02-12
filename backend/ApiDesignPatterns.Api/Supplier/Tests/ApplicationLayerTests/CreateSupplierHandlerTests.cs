// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.CreateSupplier;
using backend.Supplier.Tests.TestHelpers.Builders;
using Shouldly;
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

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("CreateSupplierAsync", 1);
        Repository.CallCount.ShouldContainKeyAndValue("CreateSupplierAddressAsync", 1);
        Repository.CallCount.ShouldContainKeyAndValue("CreateSupplierPhoneNumberAsync", 1);

        var createdSupplier = Repository.First();
        createdSupplier.Id.ShouldNotBe(0);
        createdSupplier.CreatedAt.ShouldBe(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
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

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("CreateSupplierAsync", 2);
        Repository.CallCount.ShouldContainKeyAndValue("CreateSupplierAddressAsync", 2);
        Repository.CallCount.ShouldContainKeyAndValue("CreateSupplierPhoneNumberAsync", 2);

        var firstSupplier = Repository.First(x => x.Id == firstSupplierToCreate.Id);
        firstSupplier.CreatedAt.ShouldBe(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));

        var secondSupplier = Repository.First(x => x.Id == secondSupplierToCreate.Id);
        secondSupplier.CreatedAt.ShouldBe(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    private static readonly string[] s_expected =
    [
        "CreateSupplierAsync", "CreateSupplierAddressAsync", "CreateSupplierPhoneNumberAsync"
    ];

    [Fact]
    public async Task Handle_CreatesSupplierBeforeAddressAndPhoneNumber()
    {
        var supplierToCreate = new SupplierTestDataBuilder().Build();
        var command = new CreateSupplierCommand { Supplier = supplierToCreate };
        ICommandHandler<CreateSupplierCommand> sut = CreateSupplierService();

        await sut.Handle(command);

        Repository.CallOrder.ShouldBe(s_expected);
    }
}
