// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ReviewControllers;

namespace backend.Review.Services;

public class ReviewFieldMaskConfiguration
{
    public readonly HashSet<string> ReviewFieldPaths =
    [
        "*",
        "id",
        "productid",
        "rating",
        "text",
        "createdat",
        "updatedat"
    ];

    public (
        long productId,
        decimal rating,
        string text)
        GetUpdatedReviewValues(
            UpdateReviewRequest request,
            DomainModels.Review review)
    {
        long productId = request.FieldMask.Contains("productid") && !string.IsNullOrEmpty(request.ProductId)
            ? long.Parse(request.ProductId)
            : review.ProductId;

        decimal rating = request.FieldMask.Contains("rating", StringComparer.OrdinalIgnoreCase)
                         && !string.IsNullOrEmpty(request.Rating)
            ? decimal.Parse(request.Rating)
            : review.Rating;

        string text = request.FieldMask.Contains("text", StringComparer.OrdinalIgnoreCase)
                      && !string.IsNullOrEmpty(request.Text)
            ? request.Text
            : review.Text;

        return (productId, rating, text);
    }
}
