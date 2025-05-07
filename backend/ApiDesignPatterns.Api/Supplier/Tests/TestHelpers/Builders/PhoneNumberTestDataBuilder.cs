// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels.ValueObjects;
using backend.Supplier.Tests.TestHelpers.SpecimenBuilders;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class PhoneNumberTestDataBuilder
{
    private readonly Fixture _fixture = new();
    private CountryCode _countryCode;
    private AreaCode _areaCode;
    private PhoneDigits _number;

    public PhoneNumberTestDataBuilder()
    {
        _fixture.Customizations.Add(new CountryCodeSpecimenBuilder());
        _fixture.Customizations.Add(new AreaCodeSpecimenBuilder());
        _fixture.Customizations.Add(new PhoneDigitsSpecimenBuilder());

        _countryCode = _fixture.Create<CountryCode>();
        _areaCode = _fixture.Create<AreaCode>();
        _number = _fixture.Create<PhoneDigits>();
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

    public List<PhoneNumber> BuildMany(int count)
    {
        var phoneNumbers = new List<PhoneNumber>();

        for (int i = 0; i < count; i++)
        {
            phoneNumbers.Add(new PhoneNumber
            {
                CountryCode = _fixture.Create<CountryCode>(),
                AreaCode = _fixture.Create<AreaCode>(),
                Number = _fixture.Create<PhoneDigits>(),
            });
        }

        return phoneNumbers;
    }
}
