// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.Product.Services;

public class ProductColumnMapper : IColumnMapper
{
    public string MapToColumnName(string propertyName)
    {
        return propertyName switch
        {
            "Id" => "product_id",
            "Name" => "product_name",
            "Category" => "product_category",
            "BasePrice" => "product_base_price",
            "DiscountPercentage" => "product_discount_percentage",
            "TaxRate" => "product_tax_rate",
            "Length" => "product_dimensions_length_cm",
            "Width" => "product_dimensions_width_cm",
            "Height" => "product_dimensions_height_cm",
            _ => throw new ArgumentException($"Invalid property name: {propertyName}")
        };
    }
}
