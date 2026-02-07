using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels;

public record Product
{
    public required long Id { get; init; }

    public required string Name { get; init; }

    public Category Category { get; init; }

    public required Pricing Pricing { get; init; }

    public required Dimensions Dimensions { get; init; }

    public virtual Product ApplyUpdates(UpdateProductRequest request)
    {
        string name = request.FieldMask.Contains("name", StringComparer.OrdinalIgnoreCase)
                      && !string.IsNullOrEmpty(request.Name)
            ? request.Name
            : Name;

        Category category = request.FieldMask.Contains("category", StringComparer.OrdinalIgnoreCase)
                            && Enum.TryParse(request.Category, true, out Category parsedCategory)
            ? parsedCategory
            : Category;

        var dimensions = Dimensions.ApplyUpdates(request, this);
        var pricing = Pricing.ApplyUpdates(request, this);

        return this with
        {
            Name = name,
            Category = category,
            Pricing = pricing,
            Dimensions = dimensions
        };
    }
}
