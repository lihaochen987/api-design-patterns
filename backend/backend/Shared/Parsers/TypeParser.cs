namespace backend.Shared.Parsers;

public static class TypeParser
{
    public static decimal ParseDecimal(string value, string errorMessage)
    {
        if (!decimal.TryParse(value, out var result))
        {
            throw new ArgumentException(errorMessage);
        }

        return result;
    }

    public static TEnum ParseEnum<TEnum>(string value, string errorMessage) where TEnum : struct
    {
        if (!Enum.TryParse<TEnum>(value, out var result))
        {
            throw new ArgumentException(errorMessage);
        }

        return result;
    }
}