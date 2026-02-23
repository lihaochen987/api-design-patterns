// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;

namespace backend.User.Tests.TestHelpers.Builders;

public class UserTestDataBuilder
{
    private long _id;
    private string _firstName;
    private string _lastName;
    private string _email;
    private List<long> _addressIds;
    private DateTimeOffset _createdAt;
    private List<long> _phoneNumberIds;

    public UserTestDataBuilder()
    {
        Fixture fixture = new();

        _id = fixture.Create<long>();
        _firstName = fixture.Create<string>();
        _lastName = fixture.Create<string>();
        _email = fixture.Create<string>();
        _createdAt = fixture.Create<DateTimeOffset>();
        _addressIds = fixture.Create<List<long>>();
        _phoneNumberIds = fixture.Create<List<long>>();
    }

    public UserTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public UserTestDataBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public UserTestDataBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public UserTestDataBuilder WithEmail(string email)
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
