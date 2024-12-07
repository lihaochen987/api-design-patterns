// // Licensed to the.NET Foundation under one or more agreements.
// // The.NET Foundation licenses this file to you under the MIT license.
//
// using backend.Product.DomainModels;
// using backend.Product.ProductControllers;
// using Microsoft.AspNetCore.Mvc;
// using Swashbuckle.AspNetCore.Annotations;
//
// namespace backend.Review.ReviewControllers;
//
// [ApiController]
// [Route("review")]
// public class ReplaceReviewController : ControllerBase
// {
//     [HttpPut("{id:long}")]
//     [SwaggerOperation(Summary = "Replace a review", Tags = ["Reviews"])]
//     [ProducesResponseType(typeof(ReplaceReviewResponse), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult<ReplaceProductResponse>> ReplaceReview(
//         [FromRoute] long id,
//         [FromBody] ReplaceReviewRequest request)
//     {
//         DomainModels.Review existingProduct = await productRepository.GetProductAsync(id);
//         if (existingProduct == null)
//         {
//             return NotFound();
//         }
//
//         switch (existingProduct)
//         {
//             case PetFood petFood:
//                 mapper.Map(request, petFood);
//                 break;
//
//             case GroomingAndHygiene groomingAndHygiene:
//                 mapper.Map(request, groomingAndHygiene);
//                 break;
//
//             default:
//                 mapper.Map(request, existingProduct);
//                 break;
//         }
//
//         await productRepository.UpdateProductAsync(existingProduct);
//         object response = existingProduct.Category switch
//         {
//             Category.PetFood => mapper.Map<ReplacePetFoodResponse>(existingProduct),
//             Category.GroomingAndHygiene => mapper.Map<ReplaceGroomingAndHygieneResponse>(existingProduct),
//             _ => mapper.Map<ReplaceProductResponse>(existingProduct)
//         };
//
//         return Ok(response);
//     }
// }
