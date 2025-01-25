// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.InfrastructureLayer.Queries;

public static class ProductViewQueries
{
    public const string GetProductView = """
                                         SELECT
                                             product_id AS Id,
                                             product_name AS Name,
                                             product_price AS Price,
                                             product_category_name AS Category,
                                             product_pet_food_age_group AS AgeGroup,
                                             product_pet_food_breed_size AS BreedSize,
                                             product_pet_foods_ingredients AS Ingredients,
                                             product_pet_foods_storage_instructions AS StorageInstructions,
                                             product_pet_foods_weight_kg AS WeightKg,
                                             product_grooming_and_hygiene_is_natural AS IsNatural,
                                             product_grooming_and_hygiene_is_hypoallergenic AS IsHypoallergenic,
                                             product_grooming_and_hygiene_usage_instructions AS UsageInstructions,
                                             product_grooming_and_hygiene_is_cruelty_free AS IsCrueltyFree,
                                             product_grooming_and_hygiene_safety_warnings AS SafetyWarnings,
                                             product_dimensions_length_cm AS Length,
                                             product_dimensions_width_cm AS Width,
                                             product_dimensions_height_cm AS Height
                                         FROM products_view
                                         WHERE product_id = @Id;
                                         """;

    public const string ListProductsBase = """
                                           SELECT
                                               product_id AS Id,
                                               product_name AS Name,
                                               product_price AS Price,
                                               product_category_name AS Category,
                                               product_pet_food_age_group AS AgeGroup,
                                               product_pet_food_breed_size AS BreedSize,
                                               product_pet_foods_ingredients AS Ingredients,
                                               product_pet_foods_storage_instructions AS StorageInstructions,
                                               product_pet_foods_weight_kg AS WeightKg,
                                               product_grooming_and_hygiene_is_natural AS IsNatural,
                                               product_grooming_and_hygiene_is_hypoallergenic AS IsHypoallergenic,
                                               product_grooming_and_hygiene_usage_instructions AS UsageInstructions,
                                               product_grooming_and_hygiene_is_cruelty_free AS IsCrueltyFree,
                                               product_grooming_and_hygiene_safety_warnings AS SafetyWarnings,
                                               product_dimensions_length_cm AS Length,
                                               product_dimensions_width_cm AS Width,
                                               product_dimensions_height_cm AS Height
                                           FROM products_view
                                           WHERE 1=1
                                           """;
}
