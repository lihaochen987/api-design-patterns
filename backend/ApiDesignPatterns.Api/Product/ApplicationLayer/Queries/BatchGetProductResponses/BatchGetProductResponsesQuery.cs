// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.BatchGetProductResponses;

public record BatchGetProductResponsesQuery : IQuery<Result<List<Controllers.Product.GetProductResponse>>>
{
    public required List<long> ProductIds { get; init; }
}
