// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Views;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetProductView;

public record GetProductViewQuery : IQuery<ProductView>
{
    public required long Id { get; init; }
}
