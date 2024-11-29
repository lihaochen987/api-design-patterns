using backend.Product.DomainModels;

namespace backend.Product.ViewModels;

public class ProductView
{
    public ProductView()
    {
    }

    public ProductView(
        long id,
        string name,
        decimal price,
        Category category,
        Dimensions dimensions)
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
        Dimensions = dimensions;
    }

    public long Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public Category Category { get; init; }
    public Dimensions Dimensions { get; init; }

    public AgeGroup? AgeGroup { get; init; }
    public BreedSize? BreedSize { get; init; }
    public string? Ingredients { get; init; }
    public string? NutritionalInfo { get; init; }
    public string? StorageInstructions { get; init; }
    public decimal? WeightKg { get; init; }
}