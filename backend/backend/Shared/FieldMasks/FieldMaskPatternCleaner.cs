using System.Text.RegularExpressions;

namespace backend.Shared.FieldMasks;

public partial class FieldMaskPatternCleaner : IFieldMaskPatternCleaner
{
    [GeneratedRegex(@"\b\w*response\.\b", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveResponseRegex();

    public string RemoveResponsePattern(string input)
    {
        return RemoveResponseRegex().Replace(input, "").Trim();
    }

    [GeneratedRegex("contract", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveContractRegex();

    public string RemoveContractPattern(string input)
    {
        return RemoveContractRegex().Replace(input, "").Trim();
    }
}