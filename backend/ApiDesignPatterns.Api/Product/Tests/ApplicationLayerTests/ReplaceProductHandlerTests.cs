// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.DomainModels.Enums;
using backend.Product.ProductControllers;
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
        var existingProduct = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        var replacedProduct = new ProductTestDataBuilder().WithId(existingProduct.Id).WithCategory(Category.Beds)
            .Build();
        var request = Mapper.Map<ReplaceProductRequest>(replacedProduct);
        Repository.Add(existingProduct);
        var command = new ReplaceProductCommand { ExistingProductId = existingProduct.Id, Request = request };
        ICommandHandler<ReplaceProductCommand> sut = ReplaceProductHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateProductAsync", 1);
        Repository.First().ShouldBeEquivalentTo(replacedProduct);
    }
}
