using System.ComponentModel.DataAnnotations;

namespace backend.Product.DomainModels;

public class GroomingAndHygiene : Product
{
    public bool IsNatural { get; set; }
    public bool IsHypoallergenic { get; set; }
    [MaxLength(300)] public required string UsageInstructions { get; set; }
    public bool IsCrueltyFree { get; set; }
    [MaxLength(300)] public required string SafetyWarnings { get; set; }
}
