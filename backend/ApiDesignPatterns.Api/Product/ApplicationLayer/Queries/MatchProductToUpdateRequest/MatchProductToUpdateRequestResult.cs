// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;

namespace backend.Product.ApplicationLayer.Queries.MatchProductToUpdateRequest;

public class MatchProductToUpdateRequestResult
{
    public List<(DomainModels.Product ExistingProduct, UpdateProductRequestWithId RequestProduct)> MatchedPairs
    {
        get;
        set;
    } = [];
}
