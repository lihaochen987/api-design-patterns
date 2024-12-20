// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.Review.ReviewControllers;

public class CreateReviewExtensions(TypeParser typeParser)
{
    public DomainModels.Review ToEntity(CreateReviewRequest request)
    {
        long productId = typeParser.ParseLong(request.ProductId, "Invalid product id");
        decimal rating = typeParser.ParseDecimal(request.Rating, "Invalid rating");
        string text = typeParser.ParseString(request.Text, "Invalid text");
        DateTimeOffset createdAt = typeParser.ParseDateTimeOffset(request.CreatedAt, "Invalid created at");

        return new DomainModels.Review
        {
            ProductId = productId, Rating = rating, Text = text, CreatedAt = createdAt,
        };
    }
}
