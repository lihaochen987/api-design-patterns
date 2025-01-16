// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.DeleteProduct;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.CommandService;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class DeleteProductServiceTestBase
{
    protected readonly ProductRepositoryFake Repository = [];

    protected ICommandService<DeleteProduct> DeleteProductService()
    {
        return new DeleteProductService(Repository);
    }
}
