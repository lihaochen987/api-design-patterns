// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;
using backend.Supplier.DomainModels;
using backend.Supplier.SupplierControllers;
using backend.Supplier.Tests.TestHelpers.Builders;
using Shouldly;
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

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateSupplierAsync", 1);
        Repository.CallCount.ShouldContainKeyAndValue("UpdateSupplierAddressAsync", 1);
        Repository.CallCount.ShouldContainKeyAndValue("UpdateSupplierPhoneNumberAsync", 1);
        Repository.First().FirstName.ShouldBeEquivalentTo(request.FirstName);
        Repository.First().LastName.ShouldBeEquivalentTo(supplier.LastName);
        Repository.First().Email.ShouldBeEquivalentTo(request.Email);
    }

    [Fact]
    public async Task UpdateSupplierAsync_WithNestedAddressFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        var supplier = new SupplierTestDataBuilder().Build();
        Repository.Add(supplier);
        Repository.IsDirty = false;
        var request = new UpdateSupplierRequest
        {
            Address = new AddressRequest
            {
                Street = "New Street", City = "New City", PostalCode = "New Code", Country = "New Country"
            },
            FieldMask = ["address.street", "address.postalcode"]
        };
        ICommandHandler<UpdateSupplierCommand> sut = UpdateSupplierHandler();

        await sut.Handle(new UpdateSupplierCommand { Supplier = supplier, Request = request });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateSupplierAddressAsync", 1);
        Repository.First().Address.Street.ShouldBeEquivalentTo(request.Address.Street);
        Repository.First().Address.City.ShouldBeEquivalentTo(supplier.Address.City);
        Repository.First().Address.PostalCode.ShouldBeEquivalentTo(request.Address.PostalCode);
        Repository.First().Address.Country.ShouldBeEquivalentTo(supplier.Address.Country);
    }

    [Fact]
    public async Task UpdateSupplierAsync_WithPhoneNumberFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        var supplier = new SupplierTestDataBuilder()
            .WithPhoneNumber(new PhoneNumber { CountryCode = "1", AreaCode = "555", Number = 1234567 })
            .Build();
        Repository.Add(supplier);
        Repository.IsDirty = false;

        var request = new UpdateSupplierRequest
        {
            PhoneNumber = new PhoneNumberRequest { CountryCode = "44", AreaCode = "777", Number = "9876543" },
            FieldMask = ["countrycode", "number"]
        };
        ICommandHandler<UpdateSupplierCommand> sut = UpdateSupplierHandler();

        await sut.Handle(new UpdateSupplierCommand { Supplier = supplier, Request = request });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateSupplierPhoneNumberAsync", 1);
        Repository.First().PhoneNumber.CountryCode.ShouldBeEquivalentTo(request.PhoneNumber.CountryCode);
        Repository.First().PhoneNumber.AreaCode.ShouldBeEquivalentTo(supplier.PhoneNumber.AreaCode);
        Repository.First().PhoneNumber.Number.ShouldBeEquivalentTo(long.Parse(request.PhoneNumber.Number));
    }
}
