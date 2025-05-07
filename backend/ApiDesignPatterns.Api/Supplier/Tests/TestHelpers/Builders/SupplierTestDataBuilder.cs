// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class SupplierTestDataBuilder
{
    private long _id;
    private string _firstName;
    private string _lastName;
    private string _email;
    private List<Address> _addresses;
    private DateTimeOffset _createdAt;
    private List<PhoneNumber> _phoneNumbers;

    public SupplierTestDataBuilder()
    {
        Fixture fixture = new();
        _id = fixture.Create<long>();
        _firstName = fixture.Create<string>();
        _lastName = fixture.Create<string>();
        _email = fixture.Create<string>();
        _createdAt = fixture.Create<DateTimeOffset>();
        _addresses = new AddressTestDataBuilder().BuildMany(3);
        _phoneNumbers = new PhoneNumberTestDataBuilder().BuildMany(3);
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

    public SupplierTestDataBuilder WithAddresses(List<Address> addresses)
    {
        _addresses = addresses;
        return this;
    }

    public SupplierTestDataBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public SupplierTestDataBuilder WithPhoneNumbers(List<PhoneNumber> phoneNumbers)
    {
        _phoneNumbers = phoneNumbers;
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
            Addresses = _addresses,
            CreatedAt = _createdAt,
            PhoneNumbers = _phoneNumbers
        };
    }
}
