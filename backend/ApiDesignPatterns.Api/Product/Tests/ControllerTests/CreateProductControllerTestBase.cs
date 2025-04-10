// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;
using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Product.Controllers.Product;
using backend.Product.Services.Mappers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class CreateProductControllerTestBase
{
    protected readonly ICommandHandler<CreateProductCommand> CreateProduct =
        Mock.Of<ICommandHandler<CreateProductCommand>>();

    protected readonly IQueryHandler<MapCreateProductResponseQuery, CreateProductResponse> CreateProductResponse;
    protected readonly IQueryHandler<MapCreateProductRequestQuery, DomainModels.Product> CreateProductRequest;

    protected readonly IMapper Mapper;

    protected CreateProductControllerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
        CreateProductResponse = new MapCreateProductResponseHandler(Mapper);
        CreateProductRequest = new MapCreateProductRequestHandler(Mapper);

    }

    protected CreateProductController GetCreateProductController()
    {
        return new CreateProductController(
            CreateProduct,
            CreateProductResponse,
            CreateProductRequest);
    }
}
