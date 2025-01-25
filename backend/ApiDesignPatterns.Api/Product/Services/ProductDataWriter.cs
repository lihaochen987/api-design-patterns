// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using backend.Product.DomainModels;
using backend.Product.InfrastructureLayer.Queries;
using Dapper;

namespace backend.Product.Services;

public class ProductDataWriter(IDbConnection dbConnection)
{
    public async Task<long> UpdateProduct(DomainModels.Product product, IDbTransaction transaction)
    {
        return await dbConnection.ExecuteScalarAsync<long>(
            ProductQueries.UpdateProduct,
            new
            {
                product.Id,
                product.Name,
                product.Category,
                product.Dimensions.Width,
                product.Dimensions.Height,
                product.Dimensions.Length,
                product.Pricing.BasePrice,
                product.Pricing.DiscountPercentage,
                product.Pricing.TaxRate,
            },
            transaction
        );
    }

    public async Task UpdatePetFoodProduct(PetFood product, IDbTransaction transaction)
    {
        await dbConnection.ExecuteScalarAsync(
            ProductQueries.UpdatePetFoodProduct,
            new
            {
                product.Id,
                product.AgeGroup,
                product.BreedSize,
                product.Ingredients,
                product.StorageInstructions,
                product.WeightKg
            },
            transaction
        );
    }

    public async Task UpdateGroomingAndHygieneProduct(GroomingAndHygiene product, IDbTransaction transaction)
    {
        await dbConnection.ExecuteScalarAsync(
            ProductQueries.UpdateGroomingAndHygieneProduct,
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
