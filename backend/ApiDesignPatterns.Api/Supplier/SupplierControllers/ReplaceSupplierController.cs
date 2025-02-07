using AutoMapper;
using backend.Product.ProductControllers;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.SupplierControllers;

[ApiController]
[Route("supplier")]
public class ReplaceSupplierController(
    ISupplierApplicationService applicationService,
    IQueryHandler<GetSupplierQuery, DomainModels.Supplier> getSupplier,
    IMapper mapper)
    : ControllerBase
{
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Replace a supplier", Tags = ["Suppliers"])]
    [ProducesResponseType(typeof(ReplaceSupplierResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReplaceProductResponse>> ReplaceSupplier(
        [FromRoute] long id,
        [FromBody] ReplaceSupplierRequest request)
    {
        var existingSupplier = await getSupplier.Handle(new GetSupplierQuery { Id = id });
        if (existingSupplier == null)
        {
            return NotFound();
        }

        var replacedSupplier = mapper.Map<DomainModels.Supplier>(request);

        await applicationService.ReplaceSupplierAsync(replacedSupplier, id);
        var response = mapper.Map<ReplaceSupplierResponse>(replacedSupplier);
        return Ok(response);
    }
}
