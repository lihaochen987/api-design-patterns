// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ProductControllers;
using backend.Shared.FieldMask;
using backend.Shared.FieldPath;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class GetProductControllerTestBase
{
    protected readonly Fixture Fixture;
    protected readonly IProductViewApplicationService MockApplicationService;
    private readonly IMapper _mapper;
    private readonly IFieldPathAdapter _fieldPathAdapter;
    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory;

    protected GetProductControllerTestBase()
    {
        Fixture = new Fixture();
        MockApplicationService = Mock.Of<IProductViewApplicationService>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
        _fieldPathAdapter = new FieldPathAdapter();
        _fieldMaskConverterFactory =
            new FieldMaskConverterFactory(new FieldMaskExpander(), new PropertyHandler(new NestedJObjectBuilder()));
    }

    protected GetProductController GetProductController()
    {
        return new GetProductController(
            MockApplicationService,
            _fieldPathAdapter,
            _fieldMaskConverterFactory,
            _mapper);
    }
}
