// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.DeleteProduct;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class DeleteProductServiceTests : DeleteProductServiceTestBase
{
    [Fact]
    public async Task DeleteProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToDelete = new ProductTestDataBuilder().Build();
        ICommandService<DeleteProduct> sut = DeleteProductService();

        await sut.Execute(new DeleteProduct{Product = productToDelete});

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("DeleteProductAsync", 1);
    }
}
