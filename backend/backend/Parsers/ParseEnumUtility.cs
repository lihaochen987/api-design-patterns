namespace backend.Parsers;

public static class ParseEnumUtility
{
    public static TEnum ParseEnum<TEnum>(string value, string errorMessage) where TEnum : struct
    {
        if (!Enum.TryParse<TEnum>(value, out var result))
        {
            throw new ArgumentException(errorMessage);
        }

        return result;
    }
}