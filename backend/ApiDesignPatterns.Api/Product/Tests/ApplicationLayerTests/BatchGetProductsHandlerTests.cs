// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.BatchGetProducts;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class BatchGetProductsHandlerTests : BatchGetProductsHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsSuccess_WhenAllProductsExist()
    {
        (long productId1, long productId2) = (Fixture.Create<long>(), Fixture.Create<long>());
        var productIds = new List<long> { productId1, productId2 };
        Repository.AddProduct(productId1);
        Repository.AddProduct(productId2);
        var query = new BatchGetProductsQuery { ProductIds = productIds };
        var sut = GetBatchGetProductsHandler();

        var result = await sut.Handle(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Select(p => p.Id).Should().BeEquivalentTo(productIds);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenAnyProductDoesNotExist()
    {
        (long productId1, long missingProductId) = (Fixture.Create<long>(), Fixture.Create<long>());
        Repository.AddProduct(productId1);
        var query = new BatchGetProductsQuery { ProductIds = [productId1, missingProductId] };
        var sut = GetBatchGetProductsHandler();

        var result = await sut.Handle(query);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain(missingProductId.ToString());
    }

    [Fact]
    public async Task Handle_ReturnsPetFoodProducts_WhenCategoryIsPetFood()
    {
        long productId = Fixture.Create<long>();
        var petFoodProduct = new ProductTestDataBuilder().WithId(productId).WithCategory(Category.PetFood).Build();
        Repository.Add(petFoodProduct);
        var query = new BatchGetProductsQuery { ProductIds = [productId] };
        var sut = GetBatchGetProductsHandler();

        var result = await sut.Handle(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().Should().BeOfType<PetFood>();
        result.Value.First().Should().BeSameAs(petFoodProduct);
    }

    [Fact]
    public async Task Handle_ReturnsGroomingAndHygieneProducts_WhenCategoryIsGroomingAndHygiene()
    {
        long productId = Fixture.Create<long>();
        var groomingProduct = new ProductTestDataBuilder().WithId(productId).WithCategory(Category.GroomingAndHygiene)
            .Build();
        Repository.Add(groomingProduct);
        var query = new BatchGetProductsQuery { ProductIds = [productId] };
        var sut = GetBatchGetProductsHandler();

        var result = await sut.Handle(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().Should().BeOfType<GroomingAndHygiene>();
        result.Value.First().Should().BeSameAs(groomingProduct);
    }

    [Fact]
    public async Task Handle_ReturnsBasicProducts_WhenCategoryIsNotSpecialized()
    {
        long productId = Fixture.Create<long>();
        var product = new ProductTestDataBuilder().WithId(productId).WithCategory(Category.Beds).Build();
        Repository.Add(product);
        var query = new BatchGetProductsQuery { ProductIds = [productId] };
        var sut = GetBatchGetProductsHandler();

        var result = await sut.Handle(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().Should().BeOfType<DomainModels.Product>();
        result.Value.First().Category.Should().Be(Category.Beds);
    }

    [Fact]
    public async Task Handle_ProcessesMultipleProductCategories_InSameRequest()
    {
        (long productId1, long productId2, long productId3) =
            (Fixture.Create<long>(), Fixture.Create<long>(), Fixture.Create<long>());
        Repository.AddProduct(productId1, Category.PetFood);
        Repository.AddProduct(productId2, Category.GroomingAndHygiene);
        Repository.AddProduct(productId3, Category.Beds);
        var query = new BatchGetProductsQuery { ProductIds = [productId1, productId2, productId3] };
        var sut = GetBatchGetProductsHandler();

        var result = await sut.Handle(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
        result.Value.Should().ContainSingle(p => p.Id == productId1 && p is PetFood);
        result.Value.Should().ContainSingle(p => p.Id == productId2 && p is GroomingAndHygiene);
        result.Value.Should().ContainSingle(p =>
            p.Id == productId3 && !(p is PetFood) && !(p is GroomingAndHygiene));
    }
}
