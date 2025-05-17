// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Address.DomainModels;

namespace backend.Address.Tests.TestHelpers.Builders;

public class AddressViewTestDataBuilder
{
    private readonly Fixture _fixture = new();
    private long _id;
    private long _userId;
    private string _fullAddress;

    public AddressViewTestDataBuilder()
    {
        _id = _fixture.Create<long>();
        _userId = _fixture.Create<long>();
        _fullAddress = _fixture.Create<string>();
    }

    public AddressViewTestDataBuilder WithUserId(long userId)
    {
        _userId = userId;
        return this;
    }

    public AddressViewTestDataBuilder WithFullAddress(string fullAddress)
    {
        _fullAddress = fullAddress;
        return this;
    }

    public AddressView Build()
    {
        return new AddressView { Id = _id, UserId = _userId, FullAddress = _fullAddress };
    }
}
