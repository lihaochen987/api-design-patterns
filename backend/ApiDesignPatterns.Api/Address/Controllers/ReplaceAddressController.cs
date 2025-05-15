using backend.Address.ApplicationLayer.Commands.ReplaceAddress;
using backend.Address.ApplicationLayer.Queries.GetAddress;
using backend.Product.Controllers.Product;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Address.Controllers;

[ApiController]
[Route("address")]
public class ReplaceAddressController(
    IAsyncQueryHandler<GetAddressQuery, DomainModels.Address?> getAddress,
    ICommandHandler<ReplaceAddressCommand> replaceAddress,
    IMapper mapper)
    : ControllerBase
{
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Replace an address", Tags = ["Addresses"])]
    [ProducesResponseType(typeof(ReplaceAddressResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReplaceProductResponse>> ReplaceAddress(
        [FromRoute] long id,
        [FromBody] ReplaceAddressRequest request)
    {
        var existingAddress = await getAddress.Handle(new GetAddressQuery { Id = id });
        if (existingAddress == null)
        {
            return NotFound();
        }

        await replaceAddress.Handle(new ReplaceAddressCommand { Address = mapper.Map(request, existingAddress) });

        var replacedAddress = await getAddress.Handle(new GetAddressQuery { Id = id });

        if (replacedAddress == null)
        {
            return BadRequest();
        }

        var response = mapper.Map<ReplaceAddressResponse>(replacedAddress);
        return Ok(response);
    }
}
