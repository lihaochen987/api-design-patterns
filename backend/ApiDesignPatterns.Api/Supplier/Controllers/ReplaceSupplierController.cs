using backend.Product.Controllers.Product;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Commands.ReplaceSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.Controllers;

[ApiController]
[Route("supplier")]
public class ReplaceSupplierController(
    IAsyncQueryHandler<GetSupplierQuery, DomainModels.Supplier?> getSupplier,
    ICommandHandler<ReplaceSupplierCommand> replaceSupplier,
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

        await replaceSupplier.Handle(new ReplaceSupplierCommand { Supplier = replacedSupplier });
        var response = mapper.Map<ReplaceSupplierResponse>(replacedSupplier);
        return Ok(response);
    }
}
