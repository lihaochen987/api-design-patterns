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
            FieldMask = ["firstname", "email", "lastname"]
        };
        ICommandHandler<UpdateSupplierCommand> sut = UpdateSupplierHandler();

        await sut.Handle(new UpdateSupplierCommand { Supplier = supplier, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().FirstName.Value.Should().BeEquivalentTo(request.FirstName);
        Repository.First().LastName.Value.Should().BeEquivalentTo(request.LastName);
        Repository.First().Email.Value.Should().BeEquivalentTo(request.Email);
    }

    [Fact]
    public async Task UpdateSupplierAsync_WithReferenceFieldMasks_ShouldUpdateOnlySpecifiedFields()
    {
        var supplier = new SupplierTestDataBuilder()
            .WithId(3)
            .Build();
        Repository.Add(supplier);
        Repository.IsDirty = false;
        var request = new UpdateSupplierRequest
        {
            AddressIds = [1, 2, 3],
            PhoneNumberIds = [4, 5, 6],
            FirstName = "Updated First",
            LastName = "Updated Last",
            Email = "new@email.com",
            FieldMask = ["addressids", "phonenumberids"]
        };
        ICommandHandler<UpdateSupplierCommand> sut = UpdateSupplierHandler();

        await sut.Handle(new UpdateSupplierCommand { Supplier = supplier, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().AddressIds.Should().BeEquivalentTo(request.AddressIds);
        Repository.First().PhoneNumberIds.Should().BeEquivalentTo(request.PhoneNumberIds);
        Repository.First().FirstName.Should().BeEquivalentTo(supplier.FirstName);
        Repository.First().LastName.Should().BeEquivalentTo(supplier.LastName);
        Repository.First().Email.Should().BeEquivalentTo(supplier.Email);
    }
}
