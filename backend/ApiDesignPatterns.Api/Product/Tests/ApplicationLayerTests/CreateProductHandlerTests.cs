// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Commands.CreateProduct;
using backend.Product.DomainModels.Enums;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class CreateProductHandlerTests : CreateProductHandlerTestBase
{
    [Fact]
    public async Task CreateProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToCreate = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        ICommandHandler<CreateProductQuery> sut = CreateProductService();

        await sut.Handle(new CreateProductQuery { Product = productToCreate });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("CreateProductAsync", 1);
    }

    [Fact]
    public async Task CreateProductAsync_PersistsWhenCalledTwice()
    {
        DomainModels.Product firstProductToCreate = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        DomainModels.Product secondProductToCreate =
            new ProductTestDataBuilder().WithCategory(Category.Clothing).Build();
        ICommandHandler<CreateProductQuery> sut = CreateProductService();

        await sut.Handle(new CreateProductQuery { Product = firstProductToCreate });
        await sut.Handle(new CreateProductQuery { Product = secondProductToCreate });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("CreateProductAsync", 2);
    }
}
