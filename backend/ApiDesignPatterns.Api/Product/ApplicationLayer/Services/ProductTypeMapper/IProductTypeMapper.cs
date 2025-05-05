// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.ApplicationLayer.Services.ProductTypeMapper;

public interface IProductTypeMapper
{
    TResponse MapToResponse<TResponse>(DomainModels.Product product);
}
