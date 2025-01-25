// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.InfrastructureLayer.Queries;

public static class ProductQueries
{
    public const string GetProduct = """
                                     SELECT
                                         product_id AS Id,
                                         product_name AS Name,
                                         product_category AS Category,
                                         product_dimensions_length_cm AS Length,
                                         product_dimensions_width_cm AS Width,
                                         product_dimensions_height_cm AS Height,
                                         product_base_price AS BasePrice,
                                         product_discount_percentage AS DiscountPercentage,
                                         product_tax_rate AS TaxRate
                                     FROM products
                                     WHERE product_id = @Id;
                                     """;

    public const string GetPetFoodProduct = """
                                            SELECT
                                                p.product_id AS Id,
                                                product_name AS Name,
                                                product_category AS Category,
                                                pf.product_pet_foods_age_group_id AS AgeGroup,
                                                pf.product_pet_foods_breed_size_id AS BreedSize,
                                                pf.product_pet_foods_ingredients AS Ingredients,
                                                pf.product_pet_foods_storage_instructions AS StorageInstructions,
                                                pf.product_pet_foods_weight_kg AS WeightKg,
                                                product_dimensions_length_cm AS Length,
                                                product_dimensions_width_cm AS Width,
                                                product_dimensions_height_cm AS Height,
                                                product_base_price AS BasePrice,
                                                product_discount_percentage AS DiscountPercentage,
                                                product_tax_rate AS TaxRate
                                            FROM products p
                                            JOIN public.product_pet_foods pf ON p.product_id = pf.product_id
                                            WHERE p.product_id = @Id;
                                            """;

    public const string GetGroomingAndHygieneProduct = """
                                                       SELECT
                                                           p.product_id AS Id,
                                                           product_name AS Name,
                                                           product_category AS Category,
                                                           pgah.product_grooming_and_hygiene_is_natural AS IsNatural,
                                                           pgah.product_grooming_and_hygiene_is_hypoallergenic AS IsHypoallergenic,
                                                           pgah.product_grooming_and_hygiene_usage_instructions AS UsageInstructions,
                                                           pgah.product_grooming_and_hygiene_is_cruelty_free AS IsCrueltyFree,
                                                           pgah.product_grooming_and_hygiene_safety_warnings AS SafetyWarnings,
                                                           product_dimensions_length_cm AS Length,
                                                           product_dimensions_width_cm AS Width,
                                                           product_dimensions_height_cm AS Height,
                                                           product_base_price AS BasePrice,
                                                           product_discount_percentage AS DiscountPercentage,
                                                           product_tax_rate AS TaxRate
                                                       FROM products p
                                                       JOIN public.product_grooming_and_hygiene pgah ON p.product_id = pgah.product_id
                                                       WHERE p.product_id = @Id;
                                                       """;

    public const string CreateProduct = """
                                        INSERT INTO products (
                                        product_name,
                                        product_dimensions_length_cm,
                                        product_dimensions_width_cm,
                                        product_dimensions_height_cm,
                                        product_category,
                                        product_base_price,
                                        product_discount_percentage,
                                        product_tax_rate
                                        )
                                        VALUES (
                                            @Name,
                                            @Length,
                                            @Width,
                                            @Height,
                                            @Category,
                                            @BasePrice,
                                            @DiscountPercentage,
                                            @TaxRate
                                            )
                                        RETURNING product_id;
                                        """;

    public const string CreatePetFoodProduct = """
                                               INSERT INTO product_pet_foods (
                                               product_id,
                                               product_pet_foods_age_group_id,
                                               product_pet_foods_breed_size_id,
                                               product_pet_foods_nutritional_info,
                                               product_pet_foods_ingredients,
                                               product_pet_foods_weight_kg,
                                               product_pet_foods_storage_instructions
                                               )
                                               VALUES (
                                                   @Id,
                                                   @AgeGroup,
                                                   @BreedSize,
                                                   @Ingredients,
                                                   @NutritionalInfo,
                                                   @StorageInstructions,
                                                   @WeightKg
                                                   );
                                               """;

    public const string CreateGroomingAndHygieneProduct = """
                                                          INSERT INTO product_grooming_and_hygiene (
                                                          product_id,
                                                          product_grooming_and_hygiene_is_natural,
                                                          product_grooming_and_hygiene_is_hypoallergenic,
                                                          product_grooming_and_hygiene_usage_instructions,
                                                          product_grooming_and_hygiene_is_cruelty_free,
                                                          product_grooming_and_hygiene_safety_warnings
                                                          )
                                                          VALUES (
                                                              @Id,
                                                              @IsNatural,
                                                              @IsHypoallergenic,
                                                              @UsageInstructions,
                                                              @IsCrueltyFree,
                                                              @SafetyWarnings
                                                              );
                                                          """;


    public const string DeleteProduct = """
                                        DELETE FROM products
                                        WHERE product_id = @Id;
                                        """;

    public const string UpdateProduct = """
                                        UPDATE products
                                        SET
                                        product_name = @Name,
                                        product_dimensions_length_cm = @Length,
                                        product_dimensions_width_cm = @Width,
                                        product_dimensions_height_cm = @Height,
                                        product_category = @Category,
                                        product_base_price = @BasePrice,
                                        product_discount_percentage = @DiscountPercentage,
                                        product_tax_rate = @TaxRate
                                        WHERE product_id = @Id
                                        RETURNING product_id;
                                        """;

    public const string UpdatePetFoodProduct = """
                                               UPDATE product_pet_foods
                                               SET
                                               product_pet_foods_age_group_id = @AgeGroup,
                                               product_pet_foods_breed_size_id = @BreedSize,
                                               product_pet_foods_ingredients = @Ingredients,
                                               product_pet_foods_storage_instructions = @StorageInstructions,
                                               product_pet_foods_weight_kg = @WeightKg
                                               WHERE product_id = @Id
                                               RETURNING product_id;
                                               """;

    public const string UpdateGroomingAndHygieneProduct = """
                                                          UPDATE product_grooming_and_hygiene
                                                          SET
                                                          product_grooming_and_hygiene_is_natural = @IsNatural,
                                                          product_grooming_and_hygiene_is_hypoallergenic = @IsHypoallergenic,
                                                          product_grooming_and_hygiene_usage_instructions = @UsageInstructions,
                                                          product_grooming_and_hygiene_is_cruelty_free = @IsCrueltyFree,
                                                          product_grooming_and_hygiene_safety_warnings = @SafetyWarnings
                                                          WHERE product_id = @Id
                                                          RETURNING product_id;
                                                          """;
}
