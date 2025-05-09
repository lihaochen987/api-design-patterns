// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.BatchDeleteProducts;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class BatchDeleteProductsHandlerTests : BatchDeleteProductsHandlerTestBase
{
    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectProductIds()
    {
        (long productId1, long productId2, long productId3) =
            (Fixture.Create<long>(), Fixture.Create<long>(), Fixture.Create<long>());
        Repository.AddProduct(productId1);
        Repository.AddProduct(productId2);
        Repository.AddProduct(productId3);
        ICommandHandler<BatchDeleteProductsCommand> sut = BatchDeleteProductsService();
        var command = new BatchDeleteProductsCommand { ProductIds = [productId1, productId2, productId3] };

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithEmptyIdsList_ShouldNotDeleteAnyProducts()
    {
        Repository.AddProduct();
        Repository.AddProduct();
        ICommandHandler<BatchDeleteProductsCommand> sut = BatchDeleteProductsService();
        var command = new BatchDeleteProductsCommand { ProductIds = [] };

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        Repository.Count.Should().Be(2);
    }
}
