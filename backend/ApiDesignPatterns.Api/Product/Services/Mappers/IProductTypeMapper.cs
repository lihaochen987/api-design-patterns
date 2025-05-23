﻿// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Views;

namespace backend.Product.Services.Mappers;

public interface IProductTypeMapper
{
    TResponse MapToResponse<TResponse>(DomainModels.Product product, ProductControllerMethod method);
    TResponse MapToResponse<TResponse>(ProductView product, ProductControllerMethod method);
    DomainModels.Product MapFromRequest<TRequest>(TRequest request);
}
