// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.ReplaceSupplier;
using backend.Supplier.Tests.TestHelpers.Builders;
using Shouldly;
using Xunit;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public class ReplaceSupplierHandlerTests : ReplaceSupplierHandlerTestBase
{
    [Fact]
    public async Task Handle_ReplaceSupplier_UpdatesAllSupplierInformation()
    {
        var existingSupplier = new SupplierTestDataBuilder().Build();
        var replacedSupplier = new SupplierTestDataBuilder().WithId(existingSupplier.Id).Build();
        Repository.Add(existingSupplier);
        var command = new ReplaceSupplierCommand { Supplier = replacedSupplier };
        ICommandHandler<ReplaceSupplierCommand> sut = ReplaceSupplierHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateSupplierAsync", 1);
        Repository.CallCount.ShouldContainKeyAndValue("UpdateSupplierAddressAsync", 1);
        Repository.CallCount.ShouldContainKeyAndValue("UpdateSupplierPhoneNumberAsync", 1);
        Repository.First().ShouldBeEquivalentTo(replacedSupplier);
    }
}
