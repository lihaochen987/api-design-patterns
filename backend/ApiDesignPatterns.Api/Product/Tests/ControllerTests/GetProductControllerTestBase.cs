// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Product.DomainModels.Views;
using backend.Product.ProductControllers;
using backend.Product.Queries.GetProductView;
using backend.Shared.FieldMask;
using backend.Shared.FieldPath;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class GetProductControllerTestBase
{
    protected readonly Fixture Fixture;
    protected readonly IQueryHandler<GetProductViewQuery, ProductView> MockGetProductView;
    private readonly IMapper _mapper;
    private readonly IFieldPathAdapter _fieldPathAdapter;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;

    protected GetProductControllerTestBase()
    {
        Fixture = new Fixture();
        MockGetProductView = Mock.Of<IQueryHandler<GetProductViewQuery, ProductView>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
        _fieldPathAdapter = new FieldPathAdapter();
        _fieldMaskConverterFactory =
            new FieldMaskConverterFactory(new FieldMaskExpander(), new PropertyHandler(new NestedJObjectBuilder()));
    }

    protected GetProductController GetProductController()
    {
        return new GetProductController(
            MockGetProductView,
            _fieldPathAdapter,
            _fieldMaskConverterFactory,
            _mapper);
    }
}
