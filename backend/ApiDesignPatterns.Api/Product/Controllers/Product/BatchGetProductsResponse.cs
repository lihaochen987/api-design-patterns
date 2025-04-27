// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Product.Controllers.Product;

public class BatchGetProductsResponse
{
    [Required] public required IEnumerable<GetProductResponse?> Results { get; init; } = [];
}
