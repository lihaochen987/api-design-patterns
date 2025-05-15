// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Commands.UpdateAddress;
using backend.Address.Controllers;
using backend.Address.DomainModels.ValueObjects;
using backend.Address.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.ApplicationLayerTests;

public class UpdateAddressHandlerTests : UpdateAddressHandlerTestBase
{
    [Fact]
    public async Task Handle_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        DomainModels.Address address = new AddressTestDataBuilder()
            .WithId(3)
            .WithSupplierId(42)
            .WithStreet(new Street("123 Original Street"))
            .WithCity(new City("Original City"))
            .WithPostalCode(new PostalCode("12345"))
            .WithCountry(new Country("Original Country"))
            .Build();
        Repository.Add(address);
        Repository.IsDirty = false;
        UpdateAddressRequest request = new()
        {
            SupplierId = 55,
            Street = "456 Updated Street",
            City = "Updated City",
            PostalCode = "54321",
            Country = "Updated Country",
            FieldMask = ["supplierid", "street", "city", "postalcode", "country"]
        };
        ICommandHandler<UpdateAddressCommand> sut = UpdateAddressService();

        await sut.Handle(new UpdateAddressCommand { Address = address, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().SupplierId.Should().Be(55);
        Repository.First().Street.Value.Should().Be("456 Updated Street");
        Repository.First().City.Value.Should().Be("Updated City");
        Repository.First().PostalCode.Value.Should().Be("54321");
        Repository.First().Country.Value.Should().Be("Updated Country");
    }

    [Fact]
    public async Task Handle_WithPartialFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        DomainModels.Address address = new AddressTestDataBuilder()
            .WithId(5)
            .WithSupplierId(42)
            .WithStreet(new Street("123 Original Street"))
            .WithCity(new City("Original City"))
            .WithPostalCode(new PostalCode("12345"))
            .WithCountry(new Country("Original Country"))
            .Build();
        Repository.Add(address);
        Repository.IsDirty = false;
        UpdateAddressRequest request = new()
        {
            SupplierId = 55,
            Street = "456 Updated Street",
            City = "Updated City",
            PostalCode = "54321",
            Country = "Updated Country",
            FieldMask = ["street", "postalcode"]
        };
        ICommandHandler<UpdateAddressCommand> sut = UpdateAddressService();

        await sut.Handle(new UpdateAddressCommand { Address = address, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().SupplierId.Should().Be(42);
        Repository.First().Street.Value.Should().Be("456 Updated Street");
        Repository.First().City.Value.Should().Be("Original City");
        Repository.First().PostalCode.Value.Should().Be("54321");
        Repository.First().Country.Value.Should().Be("Original Country");
    }

    [Fact]
    public async Task Handle_WithNullFieldValues_ShouldNotUpdateFields()
    {
        DomainModels.Address address = new AddressTestDataBuilder()
            .WithId(7)
            .WithSupplierId(42)
            .WithStreet(new Street("123 Original Street"))
            .WithCity(new City("Original City"))
            .WithPostalCode(new PostalCode("12345"))
            .WithCountry(new Country("Original Country"))
            .Build();
        Repository.Add(address);
        Repository.IsDirty = false;
        UpdateAddressRequest request = new()
        {
            SupplierId = null,
            Street = null,
            City = null,
            PostalCode = null,
            Country = null,
            FieldMask = ["supplierid", "street", "city", "postalcode", "country"]
        };
        ICommandHandler<UpdateAddressCommand> sut = UpdateAddressService();

        await sut.Handle(new UpdateAddressCommand { Address = address, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().SupplierId.Should().Be(42);
        Repository.First().Street.Value.Should().Be("123 Original Street");
        Repository.First().City.Value.Should().Be("Original City");
        Repository.First().PostalCode.Value.Should().Be("12345");
        Repository.First().Country.Value.Should().Be("Original Country");
    }
}
