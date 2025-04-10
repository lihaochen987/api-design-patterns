// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;

public record MapCreateProductRequestQuery : IQuery<DomainModels.Product>
{
    public required CreateProductRequest Request { get; set; }
}
