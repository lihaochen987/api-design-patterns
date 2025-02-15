// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class SupplierViewTestDataBuilder
{
    private long _id;
    private string _fullName;
    private string _email;
    private DateTimeOffset _createdAt;
    private string _street;
    private string _city;
    private string _postalCode;
    private string _country;
    private string _phoneNumber;

    public SupplierViewTestDataBuilder()
    {
        Fixture fixture = new();

        _id = fixture.Create<long>();
        _fullName = $"{fixture.Create<string>()} {fixture.Create<string>()}";
        _email = fixture.Create<string>();
        _createdAt = fixture.Create<DateTimeOffset>();
        _street = fixture.Create<string>();
        _city = fixture.Create<string>();
        _postalCode = fixture.Create<string>();
        _country = fixture.Create<string>();
        _phoneNumber = $"+{fixture.Create<int>()}-{fixture.Create<int>()}-{fixture.Create<long>()}";
    }

    public SupplierViewTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public SupplierViewTestDataBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public SupplierViewTestDataBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public SupplierViewTestDataBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public SupplierViewTestDataBuilder WithStreet(string street)
    {
        _street = street;
        return this;
    }

    public SupplierViewTestDataBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public SupplierViewTestDataBuilder WithPostalCode(string postalCode)
    {
        _postalCode = postalCode;
        return this;
    }

    public SupplierViewTestDataBuilder WithCountry(string country)
    {
        _country = country;
        return this;
    }

    public SupplierViewTestDataBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public SupplierView Build()
    {
        return new SupplierView
        {
            Id = _id,
            FullName = _fullName,
            Email = _email,
            CreatedAt = _createdAt,
            Street = _street,
            City = _city,
            PostalCode = _postalCode,
            Country = _country,
            PhoneNumber = _phoneNumber
        };
    }

    public IEnumerable<SupplierView> CreateMany(int count)
    {
        return Enumerable.Range(0, count).Select(_ => new SupplierViewTestDataBuilder().Build());
    }
}
