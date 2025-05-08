// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;
using backend.Supplier.Controllers;
using backend.Supplier.DomainModels.ValueObjects;
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

    [Fact]
    public async Task UpdateSupplierAsync_WithNestedAddressFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        var supplier = new SupplierTestDataBuilder().Build();
        Repository.Add(supplier);
        Repository.IsDirty = false;
        var addressRequest = new AddressRequest
        {
            Street = "New Street", City = "New City", PostalCode = "New Code", Country = "New Country"
        };
        var request = new UpdateSupplierRequest
        {
            Addresses = [addressRequest], FieldMask = ["address.street", "address.postalcode"]
        };
        ICommandHandler<UpdateSupplierCommand> sut = UpdateSupplierHandler();

        await sut.Handle(new UpdateSupplierCommand { Supplier = supplier, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Addresses.First().Street.Value.Should()
            .BeEquivalentTo(supplier.Addresses.First().Street.Value);
        Repository.First().Addresses.First().City.Should().BeEquivalentTo(supplier.Addresses.First().City);
        Repository.First().Addresses.First().PostalCode.Value.Should()
            .BeEquivalentTo(supplier.Addresses.First().PostalCode.Value);
        Repository.First().Addresses.First().Country.Should().BeEquivalentTo(supplier.Addresses.First().Country);
    }

    [Fact]
    public async Task UpdateSupplierAsync_WithPhoneNumberFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        var supplier = new SupplierTestDataBuilder()
            .WithPhoneNumbers([
                new DomainModels.ValueObjects.PhoneNumber
                {
                    CountryCode = new CountryCode("+1"),
                    AreaCode = new AreaCode("555"),
                    Number = new PhoneDigits(1234567)
                }
            ])
            .Build();
        Repository.Add(supplier);
        Repository.IsDirty = false;
        var phoneNumber = new PhoneNumberRequest { CountryCode = "+44", AreaCode = "777", Number = 9876543 };
        var request = new UpdateSupplierRequest { PhoneNumbers = [phoneNumber], FieldMask = ["countrycode", "number"] };
        ICommandHandler<UpdateSupplierCommand> sut = UpdateSupplierHandler();

        await sut.Handle(new UpdateSupplierCommand { Supplier = supplier, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().PhoneNumbers.First().CountryCode.Value.Should()
            .BeEquivalentTo(supplier.PhoneNumbers.First().CountryCode.Value);
        Repository.First().PhoneNumbers.First().AreaCode.Value.Should()
            .BeEquivalentTo(supplier.PhoneNumbers.First().AreaCode.ToString());
        Repository.First().PhoneNumbers.First().Number.Value.Should().Be(supplier.PhoneNumbers.First().Number.Value);
    }
}
