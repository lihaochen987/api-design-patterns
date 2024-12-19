using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Shared;

namespace backend.Product.DomainModels;

public abstract class Product : Entity
{
    private string _name = string.Empty;

    [MaxLength(100)]
    public required string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Product name is required.");
            }
            _name = value;
        }
    }

    public Category Category { get; set; }

    public required Pricing Pricing { get; set; }

    public required Dimensions Dimensions { get; set; }
}
