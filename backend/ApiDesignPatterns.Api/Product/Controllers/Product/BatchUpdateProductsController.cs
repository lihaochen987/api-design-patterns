// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.ApplicationLayer.Queries.BatchGetProducts;
using backend.Product.DomainModels;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.Product;

[ApiController]
public class BatchUpdateProductsController(
    IAsyncQueryHandler<BatchGetProductsQuery, Result<List<DomainModels.Product>>> batchGetProducts,
    ICommandHandler<UpdateProductCommand> updateProduct,
    IMapper mapper) : ControllerBase
{
    [HttpPatch("product:batchUpdate")]
    [SwaggerOperation(Summary = "Update a batch of products", Tags = ["Products"])]
    [ProducesResponseType(typeof(BatchUpdateProductsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BatchUpdateProductsResponse>> GetBatchProducts(
        [FromBody] BatchUpdateProductsRequest request)
    {
        if (request.Products is null)
        {
            return BadRequest();
        }

        var productsResult =
            await batchGetProducts.Handle(new BatchGetProductsQuery
            {
                ProductIds = request.Products.Select(x => x.Id).ToList()
            });

        if (!productsResult.IsSuccess || productsResult.Value == null)
        {
            return BadRequest(productsResult.Error);
        }

        var productsToUpdate = productsResult.Value
            .Join<DomainModels.Product, UpdateProductRequestWithId, long,
                (DomainModels.Product ExistingProduct, UpdateProductRequestWithId RequestProduct)>(
                request.Products,
                existingProduct => existingProduct.Id,
                requestProduct => requestProduct.Id,
                (existingProduct, requestProduct) =>
                    new ValueTuple<DomainModels.Product, UpdateProductRequestWithId>(existingProduct, requestProduct)
            )
            .ToList();

        foreach (var products in productsToUpdate)
        {
            await updateProduct.Handle(new UpdateProductCommand
            {
                Request = products.RequestProduct, Product = products.ExistingProduct
            });
        }

        var updatedProducts =
            await batchGetProducts.Handle(new BatchGetProductsQuery
            {
                ProductIds = request.Products.Select(x => x.Id).ToList()
            });

        var response = updatedProducts.Value!.Select(updatedProduct => updatedProduct switch
        {
            PetFood => mapper.Map<UpdatePetFoodResponse>(updatedProduct),
            GroomingAndHygiene => mapper.Map<UpdateGroomingAndHygieneResponse>(updatedProduct),
            _ => mapper.Map<UpdateProductResponse>(updatedProduct)
        });

        var result = new BatchUpdateProductsResponse { Results = response };

        return result;
    }
}
