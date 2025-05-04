// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.MatchProductToUpdateRequest;

public class MatchProductToUpdateRequestHandler :
    ISyncQueryHandler<MatchProductToUpdateRequestQuery, MatchProductToUpdateRequestResult>
{
    public MatchProductToUpdateRequestResult Handle(MatchProductToUpdateRequestQuery query)
    {
        var productsToUpdate = query.ExistingProducts
            .Join<DomainModels.Product, UpdateProductRequestWithId, long,
                (DomainModels.Product ExistingProduct, UpdateProductRequestWithId RequestProduct)>(
                query.UpdateProductRequests,
                existingProduct => existingProduct.Id,
                requestProduct => requestProduct.Id,
                (existingProduct, requestProduct) =>
                    new ValueTuple<DomainModels.Product, UpdateProductRequestWithId>(existingProduct, requestProduct)
            )
            .ToList();

        return new MatchProductToUpdateRequestResult { MatchedPairs = productsToUpdate };
    }
}
