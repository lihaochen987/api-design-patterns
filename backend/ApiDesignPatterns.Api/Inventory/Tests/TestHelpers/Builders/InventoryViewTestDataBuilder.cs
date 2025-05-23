// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.DomainModels;

namespace backend.Inventory.Tests.TestHelpers.Builders;

public class InventoryViewTestDataBuilder
{
    private long _id;
    private long _userId;
    private long _productId;
    private decimal _quantity;
    private DateTimeOffset? _restockDate;

    public InventoryViewTestDataBuilder()
    {
        Fixture fixture = new();

        _id = fixture.Create<long>();
        _userId = fixture.Create<long>();
        _productId = fixture.Create<long>();
        _quantity = fixture.Create<decimal>();
        _restockDate = fixture.Create<DateTimeOffset?>();
    }

    public InventoryViewTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public InventoryViewTestDataBuilder WithUserId(long userId)
    {
        _userId = userId;
        return this;
    }

    public InventoryViewTestDataBuilder WithProductId(long productId)
    {
        _productId = productId;
        return this;
    }

    public InventoryViewTestDataBuilder WithQuantity(decimal quantity)
    {
        _quantity = quantity;
        return this;
    }

    public InventoryViewTestDataBuilder WithRestockDate(DateTimeOffset? restockDate)
    {
        _restockDate = restockDate;
        return this;
    }

    public IEnumerable<InventoryView> CreateMany(int count)
    {
        return Enumerable.Range(1, count).Select(i => new InventoryViewTestDataBuilder().WithId(i).Build());
    }

    public InventoryView Build()
    {
        return new InventoryView
        {
            Id = _id,
            UserId = _userId,
            ProductId = _productId,
            Quantity = _quantity,
            RestockDate = _restockDate
        };
    }
}
