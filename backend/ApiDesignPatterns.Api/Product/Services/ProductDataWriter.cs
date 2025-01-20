// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using backend.Product.DomainModels;
using backend.Product.InfrastructureLayer.Queries;
using Dapper;

namespace backend.Product.Services;

public class ProductDataWriter(IDbConnection dbConnection)
{
    public async Task<long> CreateProduct(DomainModels.Product product, IDbTransaction transaction)
    {
        return await dbConnection.ExecuteScalarAsync<long>(
            ProductQueries.CreateProduct,
            new
            {
                product.Name,
                product.Dimensions.Length,
                product.Dimensions.Width,
                product.Dimensions.Height,
                product.Category,
                product.Pricing.BasePrice,
                product.Pricing.DiscountPercentage,
                product.Pricing.TaxRate
            },
            transaction
        );
    }

    public async Task CreatePetFoodProduct(PetFood product, IDbTransaction transaction)
    {
        await dbConnection.ExecuteScalarAsync(
            ProductQueries.CreatePetFoodProduct,
            new
            {
                product.Id,
                product.AgeGroup,
                product.BreedSize,
                product.Ingredients,
                product.NutritionalInfo,
                product.StorageInstructions,
                product.WeightKg
            },
            transaction
        );
    }

    public async Task CreateGroomingAndHygieneProduct(GroomingAndHygiene product, IDbTransaction transaction)
    {
        await dbConnection.ExecuteScalarAsync(
            ProductQueries.CreateGroomingAndHygieneProduct,
            new
            {
                product.Id,
                product.IsNatural,
                product.IsHypoallergenic,
                product.UsageInstructions,
                product.IsCrueltyFree,
                product.SafetyWarnings
            },
            transaction
        );
    }
}
