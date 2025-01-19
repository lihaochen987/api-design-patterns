// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer;
using backend.Product.ApplicationLayer.DeleteProduct;
using backend.Product.ProductControllers;
using backend.Shared;
using backend.Shared.CommandHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class DeleteProductControllerTestBase
{
    protected readonly ICommandHandler<DeleteProduct> MockDeleteProductHandler = Mock.Of<ICommandHandler<DeleteProduct>>();
    protected readonly IProductQueryApplicationService MockProductQueryService = Mock.Of<IProductQueryApplicationService>();

    protected DeleteProductController DeleteProductController()
    {
        return new DeleteProductController(MockDeleteProductHandler, MockProductQueryService);
    }
}
