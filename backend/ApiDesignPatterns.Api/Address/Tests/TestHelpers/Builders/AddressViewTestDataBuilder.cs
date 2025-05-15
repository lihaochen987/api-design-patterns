// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Address.DomainModels;

namespace backend.Address.Tests.TestHelpers.Builders;

public class AddressViewTestDataBuilder
{
    private readonly Fixture _fixture = new();
    private long _id;
    private long _supplierId;
    private string _fullAddress;

    public AddressViewTestDataBuilder()
    {
        _id = _fixture.Create<long>();
        _supplierId = _fixture.Create<long>();
        _fullAddress = _fixture.Create<string>();
    }

    public AddressViewTestDataBuilder WithSupplierId(long supplierId)
    {
        _supplierId = supplierId;
        return this;
    }

    public AddressViewTestDataBuilder WithFullAddress(string fullAddress)
    {
        _fullAddress = fullAddress;
        return this;
    }

    public AddressView Build()
    {
        return new AddressView { Id = _id, SupplierId = _supplierId, FullAddress = _fullAddress };
    }
}
