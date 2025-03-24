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
        Repository.CallCount.Should().ContainKey("CreateProductAsync").And.ContainValue(1);
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
        Repository.CallCount.Should().ContainKey("CreateProductAsync").And.ContainValue(2);
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
        Repository.CallCount.Should().ContainKey("CreateProductAsync").And.ContainValue(1);
        Repository.CallCount.Should().ContainKey("CreatePetFoodProductAsync").And.ContainValue(1);
    }

    [Fact]
    public async Task CreateProductAsync_DoesNotCreatePetFoodProduct_WhenCategoryIsPetFoodButTypeIsIncorrect()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.Beds)
            .Build();
        var incorrectProduct = product with { Category = Category.PetFood };
        ICommandHandler<CreateProductCommand> sut = CreateProductService();

        Func<Task> act = async () => await sut.Handle(new CreateProductCommand { Product = incorrectProduct });

        await act.Should().ThrowAsync<InvalidOperationException>();
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
        Repository.CallCount.Should().ContainKey("CreateProductAsync").And.ContainValue(1);
        Repository.CallCount.Should().ContainKey("CreateGroomingAndHygieneProductAsync").And.ContainValue(1);
    }

    [Fact]
    public async Task CreateProductAsync_DoesNotCreateGroomingProduct_WhenCategoryIsGroomingButTypeIsIncorrect()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.Beds)
            .Build();
        var incorrectProduct = product with { Category = Category.GroomingAndHygiene };
        ICommandHandler<CreateProductCommand> sut = CreateProductService();

        Func<Task> act = async () => await sut.Handle(new CreateProductCommand { Product = incorrectProduct });

        await act.Should().ThrowAsync<InvalidOperationException>();
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
        Repository.CallCount.Should().ContainKey("CreateProductAsync").And.ContainValue(1);
        Repository.CallCount.Should().NotContainKey("CreatePetFoodProductAsync");
        Repository.CallCount.Should().NotContainKey("CreateGroomingAndHygieneProductAsync");
    }
}
