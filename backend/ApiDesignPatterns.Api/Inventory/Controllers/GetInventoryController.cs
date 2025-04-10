// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryView;
using backend.Inventory.DomainModels;
using backend.Product.Controllers.Product;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Inventory.Controllers;

[ApiController]
[Route("inventory")]
public class GetInventoryController(
    IQueryHandler<GetInventoryViewQuery, InventoryView?> getInventoryView,
    IFieldMaskConverterFactory fieldMaskConverterFactory,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get inventory", Tags = ["Inventory"])]
    [ProducesResponseType(typeof(GetInventoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetProductResponse>> GetInventory(
        [FromRoute] long id,
        [FromQuery] GetInventoryRequest request)
    {
        InventoryView? inventoryView = await getInventoryView.Handle(new GetInventoryViewQuery { Id = id });
        if (inventoryView == null)
        {
            return NotFound();
        }

        var response = mapper.Map<GetInventoryResponse>(inventoryView);

        var converter = fieldMaskConverterFactory.Create(request.FieldMask);
        JsonSerializerSettings settings = new() { Converters = new List<JsonConverter> { converter } };
        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
