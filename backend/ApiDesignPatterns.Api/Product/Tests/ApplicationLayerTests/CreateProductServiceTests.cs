// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared;
using backend.Shared.CommandHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class CreateProductServiceTests : CreateProductServiceTestBase
{
    [Fact]
    public async Task CreateProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToCreate = new ProductTestDataBuilder().Build();
        ICommandHandler<ApplicationLayer.CreateProduct.CreateProductQuery> sut = CreateProductService();

        await sut.Handle(new ApplicationLayer.CreateProduct.CreateProductQuery { Product = productToCreate });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("CreateProductAsync", 1);
    }

    [Fact]
    public async Task CreateProductAsync_PersistsWhenCalledTwice()
    {
        DomainModels.Product firstProductToCreate = new ProductTestDataBuilder().Build();
        DomainModels.Product secondProductToCreate = new ProductTestDataBuilder().Build();
        ICommandHandler<ApplicationLayer.CreateProduct.CreateProductQuery> sut = CreateProductService();

        await sut.Handle(new ApplicationLayer.CreateProduct.CreateProductQuery { Product = firstProductToCreate });
        await sut.Handle(new ApplicationLayer.CreateProduct.CreateProductQuery { Product = secondProductToCreate });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("CreateProductAsync", 2);
    }
}
