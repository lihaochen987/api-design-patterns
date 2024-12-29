// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.Review.Services;

public class ReviewColumnMapper : IColumnMapper
{
    public string MapToColumnName(string propertyName)
    {
        return propertyName switch
        {
            "Id" => "review_id",
            "ProductId" => "product_id",
            "Rating" => "review_rating",
            "Text" => "review_text",
            "CreatedAt" => "review_created_at",
            "UpdatedAt" => "review_updated_at",
            _ => throw new ArgumentException($"Invalid property name: {propertyName}")
        };
    }
}
