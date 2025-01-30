using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels;

public class Product
{
    public long Id { get; set; }

    [Required] [MaxLength(100)] public required string Name { get; set; }

    [Required] public Category Category { get; set; }

    [Required] public required Pricing Pricing { get; set; }

    [Required] public required Dimensions Dimensions { get; set; }
}
