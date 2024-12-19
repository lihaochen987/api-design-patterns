using System.ComponentModel.DataAnnotations;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.DomainModels;

public class GroomingAndHygiene : Product
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private GroomingAndHygiene()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public GroomingAndHygiene(
        string name,
        Pricing pricing,
        Dimensions dimensions,
        bool isNatural,
        bool isHypoallergenic,
        string usageInstructions,
        bool isCrueltyFree,
        string safetyWarnings)
        : base(name, pricing, Category.GroomingAndHygiene, dimensions)
    {
        IsNatural = isNatural;
        IsHypoallergenic = isHypoallergenic;
        UsageInstructions = usageInstructions;
        IsCrueltyFree = isCrueltyFree;
        SafetyWarnings = safetyWarnings;
    }

    public bool IsNatural { get; private set; }
    public bool IsHypoallergenic { get; private set; }
    [MaxLength(300)] public string UsageInstructions { get; private set; }
    public bool IsCrueltyFree { get; private set; }
    [MaxLength(300)] public string SafetyWarnings { get; private set; }

    public void UpdateGroomingAndHygieneDetails(
        bool isNatural,
        bool isHypoallergenic,
        string usageInstructions,
        bool isCrueltyFree,
        string safetyWarnings)
    {
        IsNatural = isNatural;
        IsHypoallergenic = isHypoallergenic;
        UsageInstructions = usageInstructions;
        IsCrueltyFree = isCrueltyFree;
        SafetyWarnings = safetyWarnings;
    }
}
