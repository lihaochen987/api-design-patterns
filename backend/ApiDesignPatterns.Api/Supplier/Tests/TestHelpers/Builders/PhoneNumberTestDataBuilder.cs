// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class PhoneNumberTestDataBuilder
{
    private string _countryCode;
    private string _areaCode;
    private long _number;

    public PhoneNumberTestDataBuilder()
    {
        Fixture fixture = new();

        _countryCode = fixture.Create<string>();
        _areaCode = fixture.Create<string>();
        _number = fixture.Create<long>();
    }

    public PhoneNumberTestDataBuilder WithCountryCode(string countryCode)
    {
        _countryCode = countryCode;
        return this;
    }

    public PhoneNumberTestDataBuilder WithAreaCode(string areaCode)
    {
        _areaCode = areaCode;
        return this;
    }

    public PhoneNumberTestDataBuilder WithNumber(long number)
    {
        _number = number;
        return this;
    }

    public PhoneNumber Build()
    {
        return new PhoneNumber { CountryCode = _countryCode, AreaCode = _areaCode, Number = _number };
    }
}
