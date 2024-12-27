using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels;

public class Product
{
    public long Id { get; set; }
    [MaxLength(100)] public required string Name { get; set; }

    public Category Category { get; set; }

    public required Pricing Pricing { get; set; }

    public required Dimensions Dimensions { get; set; }
}
