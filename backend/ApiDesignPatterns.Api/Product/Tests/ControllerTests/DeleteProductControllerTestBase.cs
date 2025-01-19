// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Commands.DeleteProduct;
using backend.Product.ProductControllers;
using backend.Product.Queries.GetProduct;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class DeleteProductControllerTestBase
{
    protected readonly ICommandHandler<DeleteProductQuery> MockDeleteProductHandler =
        Mock.Of<ICommandHandler<DeleteProductQuery>>();

    protected readonly IQueryHandler<GetProductQuery, DomainModels.Product> MockGetProductHandler =
        Mock.Of<IQueryHandler<GetProductQuery, DomainModels.Product>>();

    protected DeleteProductController DeleteProductController()
    {
        return new DeleteProductController(MockDeleteProductHandler, MockGetProductHandler);
    }
}
