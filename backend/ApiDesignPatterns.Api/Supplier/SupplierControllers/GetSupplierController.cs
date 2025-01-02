// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.ProductControllers;
using backend.Shared.FieldMask;
using backend.Shared.FieldPath;
using backend.Supplier.ApplicationLayer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Supplier.SupplierControllers;

[ApiController]
[Route("supplier")]
public class GetSupplierController(
    ISupplierViewApplicationService applicationService,
    IFieldMaskConverterFactory fieldMaskConverterFactory,
    IFieldPathAdapter fieldPathAdapter,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get a supplier", Tags = ["Suppliers"])]
    [ProducesResponseType(typeof(GetSupplierResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetProductResponse>> GetSupplier(
        [FromRoute] long id,
        [FromQuery] GetSupplierRequest request)
    {
        var supplierView = await applicationService.GetSupplierView(id);
        if (supplierView == null)
        {
            return NotFound();
        }

        var response = mapper.Map<GetSupplierResponse>(supplierView);

        HashSet<string> validPaths = fieldPathAdapter.GetFieldPaths("Supplier");
        var converter = fieldMaskConverterFactory.Create(request.FieldMask, validPaths);
        JsonSerializerSettings settings = new() { Converters = new List<JsonConverter> { converter } };
        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
