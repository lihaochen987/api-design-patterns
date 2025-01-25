// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.Services.ProductPricingServices;
using backend.Product.Services.ProductServices;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.CommandHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class UpdateProductHandlerTestBase
{
    protected readonly ProductRepositoryFake Repository = [];
    private readonly UpdateProductTypeService _updateProductTypeService =
        new(new ProductFieldMaskConfiguration(new ProductPricingFieldMaskService(), new DimensionsFieldMaskService()));

    protected ICommandHandler<UpdateProductQuery> UpdateProductService()
    {
        return new UpdateProductHandler(Repository, _updateProductTypeService);
    }
}
