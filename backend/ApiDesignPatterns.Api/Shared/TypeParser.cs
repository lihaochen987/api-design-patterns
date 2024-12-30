using System.Text.Json;

namespace backend.Shared;

public class TypeParser
{
    public decimal ParseDecimal(string? value, string errorMessage)
    {
        if (!decimal.TryParse(value, out decimal result))
        {
            throw new ArgumentException(errorMessage);
        }

        return result;
    }

    public TEnum ParseEnum<TEnum>(string? value, string errorMessage) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException($"{typeof(TEnum).Name} is not a valid enum type");
        }

        if (!Enum.TryParse(value, true, out TEnum result))
        {
            throw new ArgumentException(errorMessage);
        }

        return result;
    }

    public bool ParseBool(bool? boolValue, string errorMessage)
    {
        if (!bool.TryParse(boolValue.ToString(), out bool boolResult))
        {
            throw new ArgumentException(errorMessage);
        }

        return boolResult;
    }

    public string ParseString(string? value, string errorMessage)
    {
        string stringValue = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException(errorMessage)
            : value;
        return stringValue;
    }

    public long ParseLong(string? value, string errorMessage)
    {
        if (!long.TryParse(value, out long result))
        {
            throw new ArgumentException(errorMessage);
        }

        return result;
    }
}
