using AutoMapper;
using backend.Supplier.ApplicationLayer;
using backend.Supplier.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.SupplierControllers;

[Route("suppliers")]
[ApiController]
public class ListSuppliersController(
    ISupplierViewApplicationService applicationService,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List suppliers", Tags = ["Suppliers"])]
    [ProducesResponseType(typeof(ListSuppliersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListSuppliersResponse>>> ListSuppliers(
        [FromQuery] ListSuppliersRequest request)
    {
        (List<SupplierView> suppliers, string? nextPageToken) = await applicationService.ListSuppliersAsync(request);

        ListSuppliersResponse response = new()
        {
            Results = mapper.Map<List<GetSupplierResponse>>(suppliers), NextPageToken = nextPageToken
        };

        return Ok(response);
    }
}
