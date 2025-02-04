// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetProductResponse;
using backend.Product.ProductControllers;
using backend.Product.Services.ProductServices;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class GetProductControllerTestBase
{
    protected readonly Fixture Fixture = new();

    protected readonly IQueryHandler<GetProductResponseQuery, GetProductResponse> MockGetProductResponse =
        Mock.Of<IQueryHandler<GetProductResponseQuery, GetProductResponse>>();

    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory =
        new FieldMaskConverterFactory(new ProductFieldPaths().ValidPaths);

    protected GetProductController GetProductController()
    {
        return new GetProductController(
            MockGetProductResponse,
            _fieldMaskConverterFactory);
    }
}
