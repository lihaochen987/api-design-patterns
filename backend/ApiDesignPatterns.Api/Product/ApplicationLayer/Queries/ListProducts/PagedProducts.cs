// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Views;

namespace backend.Product.ApplicationLayer.Queries.ListProducts;

public record PagedProducts(List<ProductView> Products, string? NextPageToken);
