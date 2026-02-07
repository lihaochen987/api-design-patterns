// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.UpdateProduct;

/// <summary>
/// Updates pet store product catalog items using field masking to selectively modify properties.
/// </summary>
/// <example>
/// Update request:
/// {
///   "name": "Premium Dog Food",
///   "fieldMask": ["name", "weightKg"],
///   "weightKg": "5.5"
/// }
/// </example>
public class UpdateProductHandler(
    IUpdateProduct repository)
    : ICommandHandler<UpdateProductCommand>
{
    /// <summary>
    /// Updates product properties specified in the field mask.
    /// </summary>
    public async Task Handle(UpdateProductCommand command)
    {
        var updatedProduct = command.Product.ApplyUpdates(command.Request);
        await repository.UpdateProductAsync(updatedProduct);
    }
}
