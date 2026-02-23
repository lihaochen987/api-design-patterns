// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;

namespace backend.Review.Tests.TestHelpers.Builders;

public class ReviewTestDataBuilder
{
    private long _id;
    private long _productId;
    private decimal _rating;
    private string _text;
    private DateTimeOffset _createdAt;
    private DateTimeOffset? _updatedAt;

    public ReviewTestDataBuilder()
    {
        Fixture fixture = new();
        fixture.Customize<decimal>(c => c.FromFactory(() => Math.Round(new Random().Next(0, 51) / 10m, 1)));

        _id = fixture.Create<long>();
        _productId = fixture.Create<long>();
        _rating = fixture.Create<decimal>();
        _text = fixture.Create<string>();
        _createdAt = fixture.Create<DateTimeOffset>();
        _updatedAt = fixture.Create<DateTimeOffset?>();
    }

    public ReviewTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public ReviewTestDataBuilder WithProductId(long productId)
    {
        _productId = productId;
        return this;
    }

    public ReviewTestDataBuilder WithRating(decimal rating)
    {
        _rating = rating;
        return this;
    }

    public ReviewTestDataBuilder WithText(string text)
    {
        _text = text;
        return this;
    }

    public ReviewTestDataBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public ReviewTestDataBuilder WithUpdatedAt(DateTimeOffset? updatedAt)
    {
        _updatedAt = updatedAt;
        return this;
    }

    public DomainModels.Review Build()
    {
        return new DomainModels.Review(_id, _productId, _rating, _text, _createdAt, _updatedAt);
    }
}
