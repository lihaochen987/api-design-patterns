using backend.Product.DomainModels;

namespace backend.Product.ViewModels;

public class ProductView(
    long id, 
    string name, 
    decimal price, 
    Category category, 
    Dimensions dimensions)
{
    public long Id { get; init; } = id;
    public string Name { get; init; } = name;
    public decimal Price { get; init; } = price;
    public Category Category { get; init; } = category;
    public Dimensions Dimensions { get; init; } = dimensions;
}