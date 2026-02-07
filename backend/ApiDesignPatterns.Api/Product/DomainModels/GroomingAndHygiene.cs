using backend.Product.Controllers.Product;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels;

public record GroomingAndHygiene : Product
{
    public required bool IsNatural { get; init; }
    public required bool IsHypoallergenic { get; init; }
    public required UsageInstructions UsageInstructions { get; init; }
    public required bool IsCrueltyFree { get; init; }
    public required string SafetyWarnings { get; init; }

    public override Product ApplyUpdates(UpdateProductRequest request)
    {
        var baseProduct = base.ApplyUpdates(request);

        bool isNatural = request.FieldMask.Contains("isnatural", StringComparer.OrdinalIgnoreCase) &&
                         request.IsNatural.HasValue
            ? request.IsNatural.Value
            : IsNatural;

        bool isHypoallergenic = request.FieldMask.Contains("ishypoallergenic", StringComparer.OrdinalIgnoreCase) &&
                                request.IsHypoAllergenic.HasValue
            ? request.IsHypoAllergenic.Value
            : IsHypoallergenic;

        var usageInstructions = request.FieldMask.Contains("usageinstructions", StringComparer.OrdinalIgnoreCase) &&
                                !string.IsNullOrEmpty(request.UsageInstructions)
            ? new UsageInstructions(request.UsageInstructions)
            : UsageInstructions;

        bool isCrueltyFree = request.FieldMask.Contains("iscrueltyfree", StringComparer.OrdinalIgnoreCase) &&
                             request.IsCrueltyFree.HasValue
            ? request.IsCrueltyFree.Value
            : IsCrueltyFree;

        string? safetyWarnings = request.FieldMask.Contains("safetywarnings", StringComparer.OrdinalIgnoreCase) &&
                                 !string.IsNullOrEmpty(request.SafetyWarnings)
            ? request.SafetyWarnings
            : SafetyWarnings;

        return (baseProduct as GroomingAndHygiene)! with
        {
            IsNatural = isNatural,
            IsHypoallergenic = isHypoallergenic,
            UsageInstructions = usageInstructions,
            IsCrueltyFree = isCrueltyFree,
            SafetyWarnings = safetyWarnings
        };
    }
}
