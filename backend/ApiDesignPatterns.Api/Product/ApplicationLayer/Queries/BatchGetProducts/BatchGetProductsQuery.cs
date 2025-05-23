﻿// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.BatchGetProducts;

public record BatchGetProductsQuery : IQuery<Result<List<DomainModels.Product>>>
{
    public required List<long> ProductIds { get; init; }
}
