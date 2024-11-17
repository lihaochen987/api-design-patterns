using backend.Product.DomainModels;

namespace backend.Product.ViewModels;

public class ProductView
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Category Category { get; set; }
    public Dimensions Dimensions { get; set; }
}