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
            "Category" => "product_category_name",
            "Price" => "product_price",
            "Length" => "product_dimensions_length_cm",
            "Width" => "product_dimensions_width_cm",
            "Height" => "product_dimensions_height_cm",
            "AgeGroup" => "product_pet_food_age_group",
            "BreedSize" => "product_pet_food_breed_size",
            "Ingredients" => "product_pet_foods_ingredients",
            "StorageInstructions" => "product_pet_foods_storage_instructions",
            "WeightKg" => "product_pet_foods_weight_kg",
            "IsNatural" => "product_grooming_and_hygiene_is_natural",
            "IsHypoallergenic" => "product_grooming_and_hygiene_is_hypoallergenic",
            "UsageInstructions" => "product_grooming_and_hygiene_usage_instructions",
            "IsCrueltyFree" => "product_grooming_and_hygiene_is_cruelty_free",
            "SafetyWarnings" => "product_grooming_and_hygiene_safety_warnings",
            _ => throw new ArgumentException($"Invalid property name: {propertyName}")
        };
    }
}
