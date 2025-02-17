using AutoMapper;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer;
using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.SupplierControllers;

[Route("suppliers")]
[ApiController]
public class ListSuppliersController(
    IQueryHandler<ListSuppliersQuery, PagedSuppliers> listSuppliers,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List suppliers", Tags = ["Suppliers"])]
    [ProducesResponseType(typeof(ListSuppliersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListSuppliersResponse>>> ListSuppliers(
        [FromQuery] ListSuppliersRequest request)
    {
        (List<SupplierView> suppliers, string? nextPageToken) =
            await listSuppliers.Handle(new ListSuppliersQuery { Request = request });

        ListSuppliersResponse response = new()
        {
            Results = mapper.Map<List<GetSupplierResponse>>(suppliers), NextPageToken = nextPageToken
        };

        return Ok(response);
    }
}
