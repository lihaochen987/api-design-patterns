// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class SupplierViewTestDataBuilder
{
    private long _id;
    private string _fullName;
    private string _email;
    private readonly DateTimeOffset _createdAt;
    private readonly List<Address> _addresses;
    private readonly List<PhoneNumber> _phoneNumbers;

    public SupplierViewTestDataBuilder()
    {
        Fixture fixture = new();

        _id = fixture.Create<long>();
        _fullName = $"{fixture.Create<string>()} {fixture.Create<string>()}";
        _email = fixture.Create<string>();
        _createdAt = fixture.Create<DateTimeOffset>();
        _addresses = new AddressTestDataBuilder().BuildMany(3);
        _phoneNumbers = new PhoneNumberTestDataBuilder().BuildMany(3);
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

    public SupplierView Build()
    {
        return new SupplierView
        {
            Id = _id,
            FullName = _fullName,
            Email = _email,
            CreatedAt = _createdAt,
            Addresses = _addresses,
            PhoneNumbers = _phoneNumbers,
        };
    }

    public IEnumerable<SupplierView> CreateMany(int count)
    {
        return Enumerable.Range(0, count).Select(_ => new SupplierViewTestDataBuilder().Build());
    }
}
