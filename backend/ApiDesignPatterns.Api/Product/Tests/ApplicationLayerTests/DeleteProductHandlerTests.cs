// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.DeleteProduct;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class DeleteProductHandlerTests : DeleteProductHandlerTestBase
{
    [Fact]
    public async Task DeleteProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToDelete = new ProductTestDataBuilder().Build();
        Repository.Add(productToDelete);
        ICommandHandler<DeleteProductCommand> sut = DeleteProductService();

        await sut.Handle(new DeleteProductCommand { Id = productToDelete.Id });

        Repository.IsDirty.Should().BeTrue();
        Repository.CallCount.Should().ContainKey("DeleteProductAsync").And.ContainValue(1);
    }
}
