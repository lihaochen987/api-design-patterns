// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Commands.DeleteProduct;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared;
using backend.Shared.CommandHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class DeleteProductHandlerTests : DeleteProductHandlerTestBase
{
    [Fact]
    public async Task DeleteProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToDelete = new ProductTestDataBuilder().Build();
        ICommandHandler<DeleteProductQuery> sut = DeleteProductService();

        await sut.Handle(new DeleteProductQuery{Product = productToDelete});

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("DeleteProductAsync", 1);
    }
}
