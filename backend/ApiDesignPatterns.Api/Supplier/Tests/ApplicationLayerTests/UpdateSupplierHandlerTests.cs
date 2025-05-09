// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;
using backend.Supplier.Controllers;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public class UpdateSupplierHandlerTests : UpdateSupplierHandlerTestBase
{
    [Fact]
    public async Task UpdateSupplierAsync_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        var supplier = new SupplierTestDataBuilder()
            .WithId(3)
            .Build();
        Repository.Add(supplier);
        Repository.IsDirty = false;
        var request = new UpdateSupplierRequest
        {
            FirstName = "Updated First",
            LastName = "Updated Last",
            Email = "new@email.com",
            FieldMask = ["firstname", "email"]
        };
        ICommandHandler<UpdateSupplierCommand> sut = UpdateSupplierHandler();

        await sut.Handle(new UpdateSupplierCommand { Supplier = supplier, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().FirstName.Should().BeEquivalentTo(request.FirstName);
        Repository.First().LastName.Should().BeEquivalentTo(supplier.LastName);
        Repository.First().Email.Should().BeEquivalentTo(request.Email);
    }
}
