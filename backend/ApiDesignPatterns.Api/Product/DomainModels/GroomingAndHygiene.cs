namespace backend.Product.DomainModels;

public record GroomingAndHygiene : Product
{
    public required bool IsNatural { get; init; }
    public required bool IsHypoallergenic { get; init; }
    public required string UsageInstructions { get; init; }
    public required bool IsCrueltyFree { get; init; }
    public required string SafetyWarnings { get; init; }
}
