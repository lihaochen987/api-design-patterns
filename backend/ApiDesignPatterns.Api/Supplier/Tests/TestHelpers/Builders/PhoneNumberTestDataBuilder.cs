// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels.ValueObjects;
using backend.Supplier.Tests.TestHelpers.SpecimenBuilders;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class PhoneNumberTestDataBuilder
{
    private CountryCode _countryCode;
    private AreaCode _areaCode;
    private PhoneDigits _number;

    public PhoneNumberTestDataBuilder()
    {
        Fixture fixture = new();
        fixture.Customizations.Add(new CountryCodeSpecimenBuilder());
        fixture.Customizations.Add(new AreaCodeSpecimenBuilder());
        fixture.Customizations.Add(new PhoneDigitsSpecimenBuilder());

        _countryCode = fixture.Create<CountryCode>();
        _areaCode = fixture.Create<AreaCode>();
        _number = fixture.Create<PhoneDigits>();
    }

    public PhoneNumberTestDataBuilder WithCountryCode(CountryCode countryCode)
    {
        _countryCode = countryCode;
        return this;
    }

    public PhoneNumberTestDataBuilder WithAreaCode(AreaCode areaCode)
    {
        _areaCode = areaCode;
        return this;
    }

    public PhoneNumberTestDataBuilder WithNumber(PhoneDigits number)
    {
        _number = number;
        return this;
    }

    public PhoneNumber Build()
    {
        return new PhoneNumber { CountryCode = _countryCode, AreaCode = _areaCode, Number = _number };
    }
}
