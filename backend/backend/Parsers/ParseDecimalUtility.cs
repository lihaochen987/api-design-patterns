namespace backend.Parsers;

public static class ParseDecimalUtility
{
    public static decimal ParseDecimal(string value, string errorMessage)
    {
        if (!decimal.TryParse(value, out var result))
        {
            throw new ArgumentException(errorMessage);
        }

        return result;
    }
}