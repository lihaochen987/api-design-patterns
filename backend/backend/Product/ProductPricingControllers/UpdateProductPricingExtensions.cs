// using System.Globalization;
// using backend.Product.DomainModels;
//
// namespace backend.Product.ProductPricingControllers;
//
// public class UpdateProductPricingExtensions
// {
//     public UpdateProductPricingResponse ToUpdateProductPricingResponse(ProductPricing productPricing)
//     {
//         return new UpdateProductPricingResponse
//         {
//             Id = productPricing.Id.ToString(),
//             BasePrice = productPricing.BasePrice.ToString(CultureInfo.InvariantCulture),
//             DiscountPercentage = productPricing.DiscountPercentage.ToString(),
//             TaxRate = productPricing.TaxRate.ToString()
//         };
//     }
// }