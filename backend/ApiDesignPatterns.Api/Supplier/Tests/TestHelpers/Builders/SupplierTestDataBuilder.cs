// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class SupplierTestDataBuilder
{
    private long _id;
    private string _firstName;
    private string _lastName;
    private string _email;
    private Address _address;
    private DateTimeOffset _createdAt;
    private PhoneNumber _phoneNumber;

    public SupplierTestDataBuilder()
    {
        Fixture fixture = new();

        _id = fixture.Create<long>();
        _firstName = fixture.Create<string>();
        _lastName = fixture.Create<string>();
        _email = fixture.Create<string>();
        _createdAt = fixture.Create<DateTimeOffset>();
        _address = new AddressTestDataBuilder().Build();
        _phoneNumber = new PhoneNumberTestDataBuilder().Build();
    }

    public SupplierTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public SupplierTestDataBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public SupplierTestDataBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public SupplierTestDataBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public SupplierTestDataBuilder WithAddress(Address address)
    {
        _address = address;
        return this;
    }

    public SupplierTestDataBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public SupplierTestDataBuilder WithPhoneNumber(PhoneNumber phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public DomainModels.Supplier Build()
    {
        return new DomainModels.Supplier
        {
            Id = _id,
            FirstName = _firstName,
            LastName = _lastName,
            Email = _email,
            Address = _address,
            CreatedAt = _createdAt,
            PhoneNumber = _phoneNumber
        };
    }
}
