using backend.Product.DomainModels.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Product.InfrastructureLayer.Database;

public class ProductPricingViewConfiguration : IEntityTypeConfiguration<ProductPricingView>
{
    public void Configure(EntityTypeBuilder<ProductPricingView> entity)
    {
        entity.ToView("products_pricing_view");

        // product_id PK
        entity.Property(e => e.Id)
            .HasColumnName("product_id");

        entity.OwnsOne(e => e.Pricing, pricing =>
        {
            // Links product_id to pricing
            pricing.WithOwner();

            // product_base_price
            pricing.Property(p => p.BasePrice)
                .HasColumnName("product_base_price");

            // product_discount_percentage
            pricing.Property(p => p.DiscountPercentage)
                .HasColumnName("product_discount_percentage");

            // product_tax_rate
            pricing.Property(p => p.TaxRate)
                .HasColumnName("product_tax_rate");
        });
    }
}
