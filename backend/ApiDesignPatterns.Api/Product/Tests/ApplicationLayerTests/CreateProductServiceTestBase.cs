// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.CreateProduct;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.CommandService;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class CreateProductServiceTestBase
{
    protected readonly ProductRepositoryFake Repository = [];

    protected ICommandService<ApplicationLayer.CreateProduct.CreateProduct> CreateProductService()
    {
        return new CreateProductService(Repository);
    }
}
