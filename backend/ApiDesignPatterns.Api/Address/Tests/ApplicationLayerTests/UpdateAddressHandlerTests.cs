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
            .WithUserId(42)
            .WithStreet(new Street("123 Original Street"))
            .WithCity(new City("Original City"))
            .WithPostalCode(new PostalCode("12345"))
            .WithCountry(new Country("Original Country"))
            .Build();
        Repository.Add(address);
        Repository.IsDirty = false;
        UpdateAddressRequest request = new()
        {
            UserId = 55,
            Street = "456 Updated Street",
            City = "Updated City",
            PostalCode = "54321",
            Country = "Updated Country",
            FieldMask = ["userid", "street", "city", "postalcode", "country"]
        };
        ICommandHandler<UpdateAddressCommand> sut = UpdateAddressService();

        await sut.Handle(new UpdateAddressCommand { Address = address, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().UserId.Should().Be(request.UserId);
        Repository.First().Street.Value.Should().Be(request.Street);
        Repository.First().City.Value.Should().Be(request.City);
        Repository.First().PostalCode.Value.Should().Be(request.PostalCode);
        Repository.First().Country.Value.Should().Be(request.Country);
    }

    [Fact]
    public async Task Handle_WithPartialFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        DomainModels.Address address = new AddressTestDataBuilder()
            .WithId(5)
            .WithUserId(42)
            .WithStreet(new Street("123 Original Street"))
            .WithPostalCode(new PostalCode("12345"))
            .Build();
        Repository.Add(address);
        Repository.IsDirty = false;
        UpdateAddressRequest request = new()
        {
            UserId = 55,
            Street = "456 Updated Street",
            City = "Updated City",
            PostalCode = "54321",
            Country = "Updated Country",
            FieldMask = ["street", "postalcode"]
        };
        ICommandHandler<UpdateAddressCommand> sut = UpdateAddressService();

        await sut.Handle(new UpdateAddressCommand { Address = address, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().UserId.Should().Be(address.UserId);
        Repository.First().Street.Value.Should().Be(request.Street);
        Repository.First().City.Should().Be(address.City);
        Repository.First().PostalCode.Value.Should().Be(request.PostalCode);
        Repository.First().Country.Should().Be(address.Country);
    }

    [Fact]
    public async Task Handle_WithNullFieldValues_ShouldNotUpdateFields()
    {
        DomainModels.Address address = new AddressTestDataBuilder()
            .WithId(7)
            .WithUserId(42)
            .WithStreet(new Street("123 Original Street"))
            .WithCity(new City("Original City"))
            .WithPostalCode(new PostalCode("12345"))
            .WithCountry(new Country("Original Country"))
            .Build();
        Repository.Add(address);
        Repository.IsDirty = false;
        UpdateAddressRequest request = new()
        {
            UserId = null,
            Street = null,
            City = null,
            PostalCode = null,
            Country = null,
            FieldMask = ["userid", "street", "city", "postalcode", "country"]
        };
        ICommandHandler<UpdateAddressCommand> sut = UpdateAddressService();

        await sut.Handle(new UpdateAddressCommand { Address = address, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().UserId.Should().Be(42);
        Repository.First().Street.Value.Should().Be("123 Original Street");
        Repository.First().City.Value.Should().Be("Original City");
        Repository.First().PostalCode.Value.Should().Be("12345");
        Repository.First().Country.Value.Should().Be("Original Country");
    }
}
