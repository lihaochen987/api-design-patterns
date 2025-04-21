// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.DeleteProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.Controllers.Product;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class DeleteProductControllerTestBase
{
    protected readonly ICommandHandler<DeleteProductCommand> MockDeleteProductHandler =
        Mock.Of<ICommandHandler<DeleteProductCommand>>();

    protected readonly IAsyncQueryHandler<GetProductQuery, DomainModels.Product?> MockGetProductHandler =
        Mock.Of<IAsyncQueryHandler<GetProductQuery, DomainModels.Product?>>();

    protected DeleteProductController DeleteProductController()
    {
        return new DeleteProductController(MockDeleteProductHandler, MockGetProductHandler);
    }
}
