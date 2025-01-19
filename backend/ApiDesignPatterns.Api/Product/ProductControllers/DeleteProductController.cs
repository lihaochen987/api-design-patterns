using backend.Product.Commands.DeleteProduct;
using backend.Product.Queries.GetProduct;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class DeleteProductController(
    ICommandHandler<DeleteProductQuery> deleteProduct,
    IQueryHandler<GetProductQuery, DomainModels.Product> getProduct)
    : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a product", Tags = ["Products"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteProduct(
        [FromRoute] long id,
        [FromQuery] DeleteProductRequest request)
    {
        DomainModels.Product? product = await getProduct.Handle(new GetProductQuery { Id = id });
        if (product == null)
        {
            return NotFound();
        }

        await deleteProduct.Handle(new DeleteProductQuery { Product = product });
        return NoContent();
    }
}
