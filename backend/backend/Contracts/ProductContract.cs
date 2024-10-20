namespace backend.Contracts;

/// <summary>
/// This abstract class is quite good to have since a separate ProductDto object is used in each of the standard
/// methods for a Product
/// </summary>
public abstract class ProductContract
{
    public string ProductName { get; set; } = "";
    public string ProductPrice { get; set; } = "";
    public string ProductCategory { get; set; } = "";
}