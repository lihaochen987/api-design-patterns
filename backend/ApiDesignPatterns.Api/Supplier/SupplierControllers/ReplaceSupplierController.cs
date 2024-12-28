// using AutoMapper;
// using backend.Product.ProductControllers;
// using backend.Supplier.ApplicationLayer;
// using Microsoft.AspNetCore.Mvc;
// using Swashbuckle.AspNetCore.Annotations;
//
// namespace backend.Supplier.SupplierControllers;
//
// [ApiController]
// [Route("supplier")]
// public class ReplaceSupplierController(
//     ISupplierApplicationService applicationService,
//     IMapper mapper)
//     : ControllerBase
// {
//     [HttpPut("{id:long}")]
//     [SwaggerOperation(Summary = "Replace a supplier", Tags = ["Suppliers"])]
//     [ProducesResponseType(typeof(ReplaceSupplierResponse), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult<ReplaceProductResponse>> ReplaceSupplier(
//         [FromRoute] long id,
//         [FromBody] ReplaceSupplierRequest request)
//     {
//         var existingSupplier = await applicationService.GetSupplierAsync(id);
//         if (existingSupplier == null)
//         {
//             return NotFound();
//         }
//
//         var replacedSupplier = mapper.Map(request, existingSupplier);
//
//         await applicationService.UpdateSupplierAsync(replacedSupplier);
//         var response = mapper.Map<ReplaceSupplierResponse>(replacedSupplier);
//         return Ok(response);
//     }
// }
