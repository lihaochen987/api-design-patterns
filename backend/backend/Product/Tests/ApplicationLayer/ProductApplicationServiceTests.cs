// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer;
using backend.Product.Tests.Helpers.Builders;
using backend.Product.Tests.Helpers.Fakes;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayer;

public class ProductApplicationServiceTests
{
    private readonly ProductRepositoryFake _repository = [];
    private readonly ProductApplicationService _service;

    public ProductApplicationServiceTests() => _service = new ProductApplicationService(_repository);

    [Fact]
    public async Task GetProductAsync_ReturnsProduct_WhenProductExists()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        _repository.Add(expectedProduct);
        _repository.IsDirty = false;

        DomainModels.Product? result = await _service.GetProductAsync(expectedProduct.Id);

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedProduct);
        _repository.IsDirty.ShouldBeFalse();
    }

    [Fact]
    public async Task GetProductAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();

        DomainModels.Product? result = await _service.GetProductAsync(expectedProduct.Id);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task CreateProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product? productToCreate = new ProductTestDataBuilder().Build();

        await _service.CreateProductAsync(productToCreate);

        _repository.IsDirty.ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product? productToDelete = new ProductTestDataBuilder().Build();

        await _service.DeleteProductAsync(productToDelete);

        _repository.IsDirty.ShouldBeTrue();
    }
}
