using System.ComponentModel.DataAnnotations;

namespace backend.Product.DomainModels;

public class GroomingAndHygiene : Product
{
    [Required] public required bool IsNatural { get; set; }
    [Required] public required bool IsHypoallergenic { get; set; }
    [Required] [MaxLength(300)] public required string UsageInstructions { get; set; }
    [Required] public required bool IsCrueltyFree { get; set; }
    [Required] [MaxLength(300)] public required string SafetyWarnings { get; set; }
}
