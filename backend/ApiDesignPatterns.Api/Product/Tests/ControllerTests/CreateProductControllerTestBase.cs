// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Queries.CreateProductResponse;
using backend.Product.ProductControllers;
using backend.Product.Services;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class CreateProductControllerTestBase
{
    protected readonly ICommandHandler<CreateProductCommand> CreateProduct =
        Mock.Of<ICommandHandler<CreateProductCommand>>();

    protected readonly IQueryHandler<CreateProductResponseQuery, CreateProductResponse> CreateProductResponse =
        Mock.Of<IQueryHandler<CreateProductResponseQuery, CreateProductResponse>>();

    protected readonly IMapper Mapper;

    protected CreateProductControllerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected CreateProductController GetCreateProductController()
    {
        return new CreateProductController(
            CreateProduct,
            CreateProductResponse,
            Mapper);
    }
}
