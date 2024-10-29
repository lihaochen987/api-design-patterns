namespace backend.Shared;

public class TypeParser
{
    public decimal ParseDecimal(string value, string errorMessage)
    {
        if (!decimal.TryParse(value, out var result))
        {
            throw new ArgumentException(errorMessage);
        }

        return result;
    }

    public TEnum ParseEnum<TEnum>(string value, string errorMessage) where TEnum : struct
    {
        if (!Enum.TryParse<TEnum>(value, out var result))
        {
            throw new ArgumentException(errorMessage);
        }

        return result;
    }
}