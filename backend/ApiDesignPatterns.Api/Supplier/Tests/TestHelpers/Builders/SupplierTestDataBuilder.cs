// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Supplier.DomainModels.ValueObjects;
using backend.Supplier.Tests.TestHelpers.SpecimenBuilders;

namespace backend.Supplier.Tests.TestHelpers.Builders;

public class SupplierTestDataBuilder
{
    private long _id;
    private FirstName _firstName;
    private LastName _lastName;
    private Email _email;
    private List<long> _addressIds;
    private DateTimeOffset _createdAt;
    private List<long> _phoneNumberIds;

    public SupplierTestDataBuilder()
    {
        Fixture fixture = new();
        fixture.Customizations.Add(new FirstNameSpecimenBuilder());
        fixture.Customizations.Add(new LastNameSpecimenBuilder());
        fixture.Customizations.Add(new EmailSpecimenBuilder());

        _id = fixture.Create<long>();
        _firstName = fixture.Create<FirstName>();
        _lastName = fixture.Create<LastName>();
        _email = fixture.Create<Email>();
        _createdAt = fixture.Create<DateTimeOffset>();
        _addressIds = fixture.Create<List<long>>();
        _phoneNumberIds = fixture.Create<List<long>>();
    }

    public SupplierTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public SupplierTestDataBuilder WithFirstName(FirstName firstName)
    {
        _firstName = firstName;
        return this;
    }

    public SupplierTestDataBuilder WithLastName(LastName lastName)
    {
        _lastName = lastName;
        return this;
    }

    public SupplierTestDataBuilder WithEmail(Email email)
    {
        _email = email;
        return this;
    }

    public SupplierTestDataBuilder WithAddressIds(List<long> addressIds)
    {
        _addressIds = addressIds;
        return this;
    }

    public SupplierTestDataBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public SupplierTestDataBuilder WithPhoneNumberIds(List<long> phoneNumberIds)
    {
        _phoneNumberIds = phoneNumberIds;
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
            AddressIds = _addressIds,
            CreatedAt = _createdAt,
            PhoneNumberIds = _phoneNumberIds
        };
    }
}
