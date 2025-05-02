// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels.ValueObjects;
using backend.Supplier.Tests.TestHelpers.SpecimenBuilders;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class AddressTestDataBuilder
{
    private Street _street;
    private City _city;
    private PostalCode _postalCode;
    private Country _country;

    public AddressTestDataBuilder()
    {
        Fixture fixture = new();

        fixture.Customizations.Add(new CitySpecimenBuilder());
        fixture.Customizations.Add(new PostalCodeSpecimenBuilder());
        fixture.Customizations.Add(new CountrySpecimenBuilder());

        _street = fixture.Create<Street>();
        _city = fixture.Create<City>();
        _postalCode = fixture.Create<PostalCode>();
        _country = fixture.Create<Country>();
    }

    public AddressTestDataBuilder WithStreet(Street street)
    {
        _street = street;
        return this;
    }

    public AddressTestDataBuilder WithCity(City city)
    {
        _city = city;
        return this;
    }

    public AddressTestDataBuilder WithPostalCode(PostalCode postalCode)
    {
        _postalCode = postalCode;
        return this;
    }

    public AddressTestDataBuilder WithCountry(Country country)
    {
        _country = country;
        return this;
    }

    public Address Build()
    {
        return new Address { Street = _street, City = _city, PostalCode = _postalCode, Country = _country };
    }
}
