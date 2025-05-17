// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.User.DomainModels.ValueObjects;
using backend.User.Tests.TestHelpers.SpecimenBuilders;

namespace backend.User.Tests.TestHelpers.Builders;

public class UserTestDataBuilder
{
    private long _id;
    private FirstName _firstName;
    private LastName _lastName;
    private Email _email;
    private List<long> _addressIds;
    private DateTimeOffset _createdAt;
    private List<long> _phoneNumberIds;

    public UserTestDataBuilder()
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

    public UserTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public UserTestDataBuilder WithFirstName(FirstName firstName)
    {
        _firstName = firstName;
        return this;
    }

    public UserTestDataBuilder WithLastName(LastName lastName)
    {
        _lastName = lastName;
        return this;
    }

    public UserTestDataBuilder WithEmail(Email email)
    {
        _email = email;
        return this;
    }

    public UserTestDataBuilder WithAddressIds(List<long> addressIds)
    {
        _addressIds = addressIds;
        return this;
    }

    public UserTestDataBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public UserTestDataBuilder WithPhoneNumberIds(List<long> phoneNumberIds)
    {
        _phoneNumberIds = phoneNumberIds;
        return this;
    }

    public DomainModels.User Build()
    {
        return new DomainModels.User
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
