// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Inventory.Controllers;

public record CreateInventoryRequest
{
    [Required] public required long UserId { get; init; }
    [Required] public required long ProductId { get; init; }
    [Required] public required decimal Quantity { get; init; }
    public DateTimeOffset? RestockDate { get; init; }
}
