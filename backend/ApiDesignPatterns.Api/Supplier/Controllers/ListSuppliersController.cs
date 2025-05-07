using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.Controllers;

[Route("suppliers")]
[ApiController]
public class ListSuppliersController(
    IAsyncQueryHandler<ListSuppliersQuery, PagedSuppliers> listSuppliers,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List suppliers", Tags = ["Suppliers"])]
    [ProducesResponseType(typeof(ListSuppliersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListSuppliersResponse>>> ListSuppliers(
        [FromQuery] ListSuppliersRequest request)
    {
        PagedSuppliers result = await listSuppliers.Handle(new ListSuppliersQuery { Request = request });

        ListSuppliersResponse response = new()
        {
            Results = mapper.Map<List<GetSupplierResponse>>(result.Suppliers), NextPageToken = result.NextPageToken
        };

        return Ok(response);
    }
}
