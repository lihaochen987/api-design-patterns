namespace backend.Product.DomainModels;

public class GroomingAndHygiene : Product
{
    public required bool IsNatural { get; set; }
    public required bool IsHypoallergenic { get; set; }
    public required string UsageInstructions { get; set; }
    public required bool IsCrueltyFree { get; set; }
    public required string SafetyWarnings { get; set; }
}
