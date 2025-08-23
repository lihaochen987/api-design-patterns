// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.DomainModels.Enums;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class CreateProductHandlerTests : CreateProductHandlerTestBase
{
    [Fact]
    public async Task CreateProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToCreate = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        ICommandHandler<CreateProductCommand> sut = CreateProductService();

        await sut.Handle(new CreateProductCommand { Product = productToCreate });

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().Contain(productToCreate);
    }

    [Fact]
    public async Task CreateProductAsync_PersistsWhenCalledTwice()
    {
        DomainModels.Product firstProductToCreate = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        DomainModels.Product secondProductToCreate =
            new ProductTestDataBuilder().WithCategory(Category.Clothing).Build();
        ICommandHandler<CreateProductCommand> sut = CreateProductService();

        await sut.Handle(new CreateProductCommand { Product = firstProductToCreate });
        await sut.Handle(new CreateProductCommand { Product = secondProductToCreate });

        Repository.IsDirty.Should().BeTrue();
        Repository.Count.Should().Be(2);
        Repository.Should().Contain(firstProductToCreate);
        Repository.Should().Contain(secondProductToCreate);
    }

    [Fact]
    public async Task CreateProductAsync_CreatesPetFoodProduct_WhenCategoryIsPetFood()
    {
        var petFoodProduct = new ProductTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();
        ICommandHandler<CreateProductCommand> sut = CreateProductService();

        await sut.Handle(new CreateProductCommand { Product = petFoodProduct });

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().Contain(petFoodProduct);
    }

    [Fact]
    public async Task CreateProductAsync_CreatesGroomingAndHygieneProduct_WhenCategoryIsGroomingAndHygiene()
    {
        var groomingProduct = new ProductTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        ICommandHandler<CreateProductCommand> sut = CreateProductService();

        await sut.Handle(new CreateProductCommand { Product = groomingProduct });

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().Contain(groomingProduct);
    }

    [Fact]
    public async Task CreateProductAsync_DoesNotCallSpecificCategoryMethods_ForOtherCategories()
    {
        var genericProduct = new ProductTestDataBuilder()
            .WithCategory(Category.Beds)
            .Build();
        ICommandHandler<CreateProductCommand> sut = CreateProductService();

        await sut.Handle(new CreateProductCommand { Product = genericProduct });

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().Contain(genericProduct);
    }
}
