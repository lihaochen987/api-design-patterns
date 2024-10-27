using System.Text.RegularExpressions;

namespace backend;

public static partial class StringHelper
{
    public static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsUpper(input[0]))
            return input;

        return char.ToUpper(input[0]) + input[1..];
    }

    public static string RemoveResponseFromString(string input)
    {
        return RemoveResponseRegex().Replace(input, "").Trim();
    }

    public static string RemoveContractFromString(string input)
    {
        return RemoveContractRegex().Replace(input, "").Trim();
    }

    [GeneratedRegex(@"\b\w*response\.\b", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveResponseRegex();

    [GeneratedRegex("contract", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveContractRegex();
}