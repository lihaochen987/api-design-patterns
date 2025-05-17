// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.DomainModels.ValueObjects;

namespace backend.Inventory.Tests.TestHelpers.Builders;

public class InventoryTestDataBuilder
{
    private long _id;
    private long _userId;
    private long _productId;
    private Quantity _quantity;
    private DateTimeOffset? _restockDate;

    public InventoryTestDataBuilder()
    {
        Fixture fixture = new();

        _id = fixture.Create<long>();
        _userId = fixture.Create<long>();
        _productId = fixture.Create<long>();
        _quantity = fixture.Create<Quantity>();
        _restockDate = fixture.Create<DateTimeOffset?>();
    }

    public InventoryTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public InventoryTestDataBuilder WithUserId(long userId)
    {
        _userId = userId;
        return this;
    }

    public InventoryTestDataBuilder WithProductId(long productId)
    {
        _productId = productId;
        return this;
    }

    public InventoryTestDataBuilder WithQuantity(Quantity quantity)
    {
        _quantity = quantity;
        return this;
    }

    public InventoryTestDataBuilder WithRestockDate(DateTimeOffset? restockDate)
    {
        _restockDate = restockDate;
        return this;
    }

    public DomainModels.Inventory Build()
    {
        return new DomainModels.Inventory
        {
            Id = _id,
            UserId = _userId,
            ProductId = _productId,
            Quantity = _quantity,
            RestockDate = _restockDate
        };
    }
}
