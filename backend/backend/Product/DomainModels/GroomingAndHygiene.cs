namespace backend.Product.DomainModels;

public class GroomingAndHygiene : Product
{
    private GroomingAndHygiene()
    {
    }

    public GroomingAndHygiene(
        long id,
        string name,
        Pricing pricing,
        Dimensions dimensions,
        bool isNatural,
        bool isHypoallergenic,
        string usageInstructions,
        bool isCrueltyFree,
        string safetyWarnings)
        : base(id, name, pricing, Category.GroomingAndHygiene, dimensions)
    {
        IsNatural = isNatural;
        IsHypoallergenic = isHypoallergenic;
        UsageInstructions = usageInstructions;
        IsCrueltyFree = isCrueltyFree;
        SafetyWarnings = safetyWarnings;
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
    public string UsageInstructions { get; private set; }
    public bool IsCrueltyFree { get; private set; }
    public string SafetyWarnings { get; private set; }

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