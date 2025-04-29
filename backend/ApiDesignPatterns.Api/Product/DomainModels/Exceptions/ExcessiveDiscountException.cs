// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.Exceptions;

/// <summary>
/// Exception thrown when a high-value product's pricing would result in an excessive effective discount.
/// </summary>
public class ExcessiveDiscountException(
    decimal effectiveDiscount,
    decimal maximumDiscount)
    : ArgumentException(
        $"Excessive effective discount: {effectiveDiscount:F2}%. Maximum allowed: {maximumDiscount:F2}%");
