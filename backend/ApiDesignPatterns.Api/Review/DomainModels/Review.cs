// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.DomainModels;

public record Review
{
    public Review(
        long id,
        long productId,
        decimal rating,
        string text,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt
    )
    {
        if (rating is < 0 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(rating));
        }

        Id = id;
        ProductId = productId;
        Rating = rating;
        Text = text;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public long Id { get; private set; }
    public long ProductId { get; private set; }
    public decimal Rating { get; private set; }
    public string Text { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
}
