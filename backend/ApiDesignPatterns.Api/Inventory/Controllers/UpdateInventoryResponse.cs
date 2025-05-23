// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Inventory.Controllers;

public class UpdateInventoryResponse
{
    [Required] public required string UserId { get; init; }
    [Required] public required string ProductId { get; init; }
    [Required] public required string Quantity { get; init; }
    public string? RestockDate { get; init; }
}
