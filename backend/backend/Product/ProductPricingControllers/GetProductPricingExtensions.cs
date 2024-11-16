// using System.Globalization;
// using backend.Product.DomainModels;
//
// namespace backend.Product.ProductPricingControllers;
//
// public class GetProductPricingExtensions
// {
//     public GetProductPricingResponse ToGetProductPricingResponse(ProductPricing productPricing)
//     {
//         return new GetProductPricingResponse
//         {
//             Id = productPricing.Id.ToString(),
//             BasePrice = productPricing.BasePrice.ToString(CultureInfo.InvariantCulture),
//             DiscountPercentage = productPricing.DiscountPercentage.ToString(),
//             TaxRate = productPricing.TaxRate.ToString(),
//         };
//     }
// }