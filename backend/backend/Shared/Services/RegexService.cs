using System.Text.RegularExpressions;

namespace backend.Shared.Services;

public partial class RegexService : IRegexService
{
    [GeneratedRegex(@"\b\w*response\.\b", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveResponseRegex();

    public string RemoveHcLc(string input)
    {
        return RemoveResponseRegex().Replace(input, "").Trim();
    }

    [GeneratedRegex("contract", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveContractRegex();

    public string RemoveContractFromString(string input)
    {
        return RemoveContractRegex().Replace(input, "").Trim();
    }
}