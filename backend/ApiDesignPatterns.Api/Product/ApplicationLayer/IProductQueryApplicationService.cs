// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.ApplicationLayer;

public interface IProductQueryApplicationService
{
    Task<DomainModels.Product?> GetProductAsync(long id);
}
