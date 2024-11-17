using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Product.Database;

public class ProductViewBuilder
{
    private readonly EntityTypeBuilder<DomainModels.Product> _entity;

    public ProductViewBuilder(EntityTypeBuilder<DomainModels.Product> entity)
    {
        _entity = entity;
    }
    
    public ProductViewBuilder MapToTable(string tableName)
    {
        _entity.ToTable(tableName);
        return this;
    }

    public ProductViewBuilder WithPrimaryKey()
    {
        _entity.HasKey(p => p.Id);
        _entity.Property(p => p.Id).HasColumnName("product_id");
        return this;
    }

    public ProductViewBuilder WithName()
    {
        _entity.Property(p => p.Name).HasColumnName("product_name");
        return this;
    }

    public ProductViewBuilder WithDimensions()
    {
        _entity.OwnsOne(p => p.Dimensions, dimensions =>
        {
            dimensions.Property(d => d.Length).HasColumnName("product_dimensions_length");
            dimensions.Property(d => d.Width).HasColumnName("product_dimensions_width");
            dimensions.Property(d => d.Height).HasColumnName("product_dimensions_height");
        });
        return this;
    }

    public ProductViewBuilder WithCategory()
    {
        _entity.Property(p => p.Category)
            .HasColumnName("product_category_name")
            .HasConversion<string>();
        return this;
    }

    public ProductViewBuilder WithPricing()
    {
        _entity.OwnsOne(p => p.Pricing, pricing =>
        {
            pricing.Property(p => p.BasePrice).HasColumnName("product_base_price");
            pricing.Property(p => p.DiscountPercentage).HasColumnName("product_discount_percentage");
            pricing.Property(p => p.TaxRate).HasColumnName("product_tax_rate");
        });
        return this;
    }

    public ProductViewBuilder WithCalculatedPrice()
    {
        _entity.Property(p => p.Price).HasColumnName("product_price");
        return this;
    }
}
