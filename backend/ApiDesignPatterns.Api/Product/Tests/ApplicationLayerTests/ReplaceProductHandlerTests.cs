// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class ReplaceProductHandlerTests : ReplaceProductHandlerTestBase
{
    [Fact]
    public async Task Handle_UpdatesProduct_WhenValidProductIsProvided()
    {
        var existingProduct = new ProductTestDataBuilder().Build();
        Repository.Add(existingProduct);
        var replacedProduct = new ProductTestDataBuilder().WithId(existingProduct.Id).Build();
        var command = new ReplaceProductCommand { Product = replacedProduct };
        ICommandHandler<ReplaceProductCommand> sut = ReplaceProductHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateProductAsync", 1);
    }
}
