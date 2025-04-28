// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.DomainModels.ValueObjects;
using backend.Review.Tests.TestHelpers.SpecimenBuilders;

namespace backend.Review.Tests.TestHelpers.Builders;

public class ReviewTestDataBuilder
{
    private long _id;
    private long _productId;
    private Rating _rating;
    private Text _text;
    private DateTimeOffset _createdAt;
    private DateTimeOffset? _updatedAt;

    public ReviewTestDataBuilder()
    {
        Fixture fixture = new();

        fixture.Customizations.Add(new RatingSpecimenBuilder());

        _id = fixture.Create<long>();
        _productId = fixture.Create<long>();
        _rating = fixture.Create<Rating>();
        _text = fixture.Create<Text>();
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

    public ReviewTestDataBuilder WithRating(Rating rating)
    {
        _rating = rating;
        return this;
    }

    public ReviewTestDataBuilder WithText(Text text)
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
        return new DomainModels.Review
        {
            Id = _id,
            ProductId = _productId,
            Rating = _rating,
            Text = _text,
            CreatedAt = _createdAt,
            UpdatedAt = _updatedAt
        };
    }
}
