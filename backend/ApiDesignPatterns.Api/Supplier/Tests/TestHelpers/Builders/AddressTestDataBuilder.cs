// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class AddressTestDataBuilder
{
    private string _street;
    private string _city;
    private string _postalCode;
    private string _country;

    public AddressTestDataBuilder()
    {
        Fixture fixture = new();

        _street = fixture.Create<string>();
        _city = fixture.Create<string>();
        _postalCode = fixture.Create<string>();
        _country = fixture.Create<string>();
    }

    public AddressTestDataBuilder WithStreet(string street)
    {
        _street = street;
        return this;
    }

    public AddressTestDataBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public AddressTestDataBuilder WithPostalCode(string postalCode)
    {
        _postalCode = postalCode;
        return this;
    }

    public AddressTestDataBuilder WithCountry(string country)
    {
        _country = country;
        return this;
    }

    public Address Build()
    {
        return new Address { Street = _street, City = _city, PostalCode = _postalCode, Country = _country };
    }
}
