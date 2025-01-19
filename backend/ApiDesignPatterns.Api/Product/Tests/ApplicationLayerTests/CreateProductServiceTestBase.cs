// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.CreateProduct;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.CommandHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class CreateProductServiceTestBase
{
    protected readonly ProductRepositoryFake Repository = [];

    protected ICommandHandler<ApplicationLayer.CreateProduct.CreateProductQuery> CreateProductService()
    {
        return new CreateProductHandler(Repository);
    }
}
