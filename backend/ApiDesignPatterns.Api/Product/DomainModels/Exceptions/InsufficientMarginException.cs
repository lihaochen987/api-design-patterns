// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.Exceptions;

/// <summary>
/// Exception thrown when a product's pricing would result in an insufficient profit margin.
/// </summary>
public class InsufficientMarginException(
    decimal actualMargin,
    decimal requiredMargin)
    : ArgumentException($"Insufficient profit margin: {actualMargin:F2}%. Minimum required: {requiredMargin:F2}%");
