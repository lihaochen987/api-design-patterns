// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using backend.Product.Controllers.Product;

namespace backend.Inventory.Controllers;

public class ListSupplierProductsResponse
{
    [Required] public required IEnumerable<GetProductResponse?> Results { get; init; } = [];
    public string? NextPageToken { get; init; }
}
