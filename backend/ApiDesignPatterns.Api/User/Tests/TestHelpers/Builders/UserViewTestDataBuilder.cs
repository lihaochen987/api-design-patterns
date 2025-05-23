// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.User.DomainModels;

namespace backend.User.Tests.TestHelpers.Builders;

public class UserViewTestDataBuilder
{
    private long _id;
    private string _fullName;
    private string _email;
    private readonly DateTimeOffset _createdAt;
    private readonly List<long> _addressIds;
    private readonly List<long> _phoneNumberIds;

    public UserViewTestDataBuilder()
    {
        Fixture fixture = new();

        _id = fixture.Create<long>();
        _fullName = $"{fixture.Create<string>()} {fixture.Create<string>()}";
        _email = fixture.Create<string>();
        _createdAt = fixture.Create<DateTimeOffset>();
        _addressIds = fixture.Create<List<long>>();
        _phoneNumberIds = fixture.Create<List<long>>();
    }

    public UserViewTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public UserViewTestDataBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public UserViewTestDataBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserView Build()
    {
        return new UserView
        {
            Id = _id,
            FullName = _fullName,
            Email = _email,
            CreatedAt = _createdAt,
            AddressIds = _addressIds,
            PhoneNumberIds = _phoneNumberIds,
        };
    }

    public IEnumerable<UserView> CreateMany(int count)
    {
        return Enumerable.Range(1, count).Select(_ => new UserViewTestDataBuilder().Build());
    }
}
