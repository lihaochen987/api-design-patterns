// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class MapCreateProductRequestHandlerTests : MapCreateProductRequestHandlerTestBase
{
    [Fact]
    public void Handle_ReturnsPetFood_WhenCategoryIsPetFood()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();
        var request = Mapper.Map<CreateProductRequest>(product);
        var query = new MapCreateProductRequestQuery { Request = request };
        var sut = GetMapCreateProductRequestHandler();

        DomainModels.Product result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<PetFood>();
        result.Should().BeEquivalentTo(product, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public void Handle_ReturnsGroomingAndHygiene_WhenCategoryIsGroomingAndHygiene()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        var request = Mapper.Map<CreateProductRequest>(product);
        var query = new MapCreateProductRequestQuery { Request = request };
        var sut = GetMapCreateProductRequestHandler();

        DomainModels.Product result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<GroomingAndHygiene>();
        result.Should().BeEquivalentTo(product, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public void Handle_ReturnsBaseProduct_WhenCategoryIsNotSpecialized()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.Beds)
            .Build();
        var request = Mapper.Map<CreateProductRequest>(product);
        var query = new MapCreateProductRequestQuery { Request = request };
        var sut = GetMapCreateProductRequestHandler();

        DomainModels.Product result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<DomainModels.Product>();
        result.Should().BeEquivalentTo(product, options => options.Excluding(x => x.Id));
    }
}
