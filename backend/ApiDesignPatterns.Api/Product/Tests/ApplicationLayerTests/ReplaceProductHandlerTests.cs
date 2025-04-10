// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class ReplaceProductHandlerTests : ReplaceProductHandlerTestBase
{
    [Fact]
    public async Task Handle_ReplaceProduct_WhenValidProductIsProvided()
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

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().Contain(replacedProduct);
    }

    [Fact]
    public async Task Handle_ReplacePetFoodProduct_WhenPetFoodCategoryIsProvided()
    {
        var existingProduct = new ProductTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();
        var replacedProduct = new ProductTestDataBuilder()
            .WithCategory(Category.PetFood)
            .WithId(existingProduct.Id)
            .Build();
        var request = Mapper.Map<ReplaceProductRequest>(replacedProduct);
        Repository.Add(existingProduct);
        var command = new ReplaceProductCommand { ExistingProductId = existingProduct.Id, Request = request };
        ICommandHandler<ReplaceProductCommand> sut = ReplaceProductHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().ContainEquivalentOf(replacedProduct);
    }

    [Fact]
    public async Task Handle_ReplaceGroomingProduct_WhenGroomingCategoryIsProvided()
    {
        var existingProduct = new ProductTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        var replacedProduct = new ProductTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .WithId(existingProduct.Id)
            .Build();
        var request = Mapper.Map<ReplaceProductRequest>(replacedProduct);
        Repository.Add(existingProduct);
        var command = new ReplaceProductCommand { ExistingProductId = existingProduct.Id, Request = request };
        ICommandHandler<ReplaceProductCommand> sut = ReplaceProductHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().Contain(replacedProduct);
    }

    [Fact]
    public async Task Handle_PreservesExistingId_WhenReplacingProduct()
    {
        long existingId = Fixture.Create<long>();
        long newId = Fixture.Create<long>();
        var existingProduct = new ProductTestDataBuilder()
            .WithId(existingId)
            .Build();
        var replacedProduct = new ProductTestDataBuilder()
            .WithId(newId)
            .Build();
        var request = Mapper.Map<ReplaceProductRequest>(replacedProduct);
        Repository.Add(existingProduct);
        var command = new ReplaceProductCommand { ExistingProductId = existingId, Request = request };
        ICommandHandler<ReplaceProductCommand> sut = ReplaceProductHandler();

        await sut.Handle(command);

        Repository.First().Id.Should().Be(existingId);
    }
}
