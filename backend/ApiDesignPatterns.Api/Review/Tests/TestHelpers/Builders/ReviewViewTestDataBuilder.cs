// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.DomainModels;

namespace backend.Review.Tests.TestHelpers.Builders;

public class ReviewViewTestDataBuilder
{
    private readonly Fixture _fixture;
    private long? _id;
    private long _productId;
    private decimal _rating;
    private string _text;
    private DateTimeOffset _createdAt;
    private DateTimeOffset? _updatedAt;

    public ReviewViewTestDataBuilder()
    {
        // Fixture configuration
        _fixture = new Fixture();

        // Generate default test values
        _productId = _fixture.Create<long>();
        _rating = _fixture.Create<decimal>() % 5 + 1; // Rating between 1-5
        _text = _fixture.Create<string>();
        _createdAt = _fixture.Create<DateTimeOffset>();
        _updatedAt = null;
    }

    public ReviewViewTestDataBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public ReviewViewTestDataBuilder WithProductId(long productId)
    {
        _productId = productId;
        return this;
    }

    public ReviewViewTestDataBuilder WithRating(decimal rating)
    {
        _rating = rating;
        return this;
    }

    public ReviewViewTestDataBuilder WithText(string text)
    {
        _text = text;
        return this;
    }

    public ReviewViewTestDataBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public ReviewViewTestDataBuilder WithUpdatedAt(DateTimeOffset? updatedAt)
    {
        _updatedAt = updatedAt;
        return this;
    }

    public IEnumerable<ReviewView> CreateMany(int count)
    {
        var reviews = new List<ReviewView>();

        for (int i = 0; i < count; i++)
        {
            reviews.Add(Build());
        }

        return reviews;
    }

    public ReviewView Build() =>
        new()
        {
            Id = _id ?? _fixture.Create<long>(),
            ProductId = _productId,
            Rating = _rating,
            Text = _text,
            CreatedAt = _createdAt,
            UpdatedAt = _updatedAt
        };
}
