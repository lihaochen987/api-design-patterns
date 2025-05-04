// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.MatchProductToUpdateRequest;

public class MatchProductToUpdateRequestQuery : IQuery<MatchProductToUpdateRequestResult>
{
    public required IEnumerable<DomainModels.Product> ExistingProducts { get; init; }
    public required IEnumerable<UpdateProductRequestWithId> UpdateProductRequests { get; init; }
}
