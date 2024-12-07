// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.DomainModels;

using System.ComponentModel.DataAnnotations;

public class Review
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Review()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public Review(
        long id,
        long productId,
        decimal rating,
        string text,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt = null
    )
    {
        EnforceInvariants(rating, text);
        Id = id;
        ProductId = productId;
        Rating = rating;
        Text = text;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Review(
        long productId,
        decimal rating,
        string text
    )
    {
        EnforceInvariants(rating, text);
        ProductId = productId;
        Rating = rating;
        Text = text;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = null;
    }

    public long Id { get; private set; }

    public long ProductId { get; private set; }

    [Range(0, 5, ErrorMessage = "Review rating must be between 0 and 5.")]
    public decimal Rating { get; private set; }

    [MaxLength(5000, ErrorMessage = "Review text must be 5000 characters or less.")]
    public string Text { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

    public void UpdateReview(decimal reviewRating, string reviewText)
    {
        EnforceInvariants(reviewRating, reviewText);
        Rating = reviewRating;
        Text = reviewText;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    private static void EnforceInvariants(decimal reviewRating, string reviewText)
    {
        if (reviewRating is < 0 or > 5)
        {
            throw new ArgumentException("Review rating must be between 0 and 5.");
        }

        if (!string.IsNullOrWhiteSpace(reviewText) && reviewText.Length > 5000)
        {
            throw new ArgumentException("Review text must be 5000 characters or less.");
        }
    }
}
