// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels.ValueObjects;
using backend.Supplier.Tests.TestHelpers.SpecimenBuilders;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class AddressTestDataBuilder
{
    private readonly Fixture _fixture = new();
    private Street _street;
    private City _city;
    private PostalCode _postalCode;
    private Country _country;

    public AddressTestDataBuilder()
    {
        _fixture.Customizations.Add(new CitySpecimenBuilder());
        _fixture.Customizations.Add(new PostalCodeSpecimenBuilder());
        _fixture.Customizations.Add(new CountrySpecimenBuilder());

        _street = _fixture.Create<Street>();
        _city = _fixture.Create<City>();
        _postalCode = _fixture.Create<PostalCode>();
        _country = _fixture.Create<Country>();
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

    public List<Address> BuildMany(int count)
    {
        var addresses = new List<Address>();

        for (int i = 0; i < count; i++)
        {
            addresses.Add(new Address
            {
                Street = _fixture.Create<Street>(),
                City = _fixture.Create<City>(),
                PostalCode = _fixture.Create<PostalCode>(),
                Country = _fixture.Create<Country>()
            });
        }

        return addresses;
    }
}
