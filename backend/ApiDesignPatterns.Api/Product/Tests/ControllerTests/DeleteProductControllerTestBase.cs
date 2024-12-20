// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer;
using backend.Product.ProductControllers;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class DeleteProductControllerTestBase
{
    protected readonly IProductApplicationService MockApplicationService = Mock.Of<IProductApplicationService>();

    protected DeleteProductController DeleteProductController()
    {
        return new DeleteProductController(MockApplicationService);
    }
}
