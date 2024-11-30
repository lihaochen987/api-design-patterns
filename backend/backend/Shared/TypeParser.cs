using System.Text.Json;

namespace backend.Shared;

public class TypeParser
{
    public decimal ParseDecimal(string? value, string errorMessage)
    {
        if (!decimal.TryParse(value, out var result))
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

        if (!Enum.TryParse(value, ignoreCase: true, out TEnum result))
        {
            throw new ArgumentException(errorMessage);
        }

        return result;
    }

    public string ParseDictionaryToString(Dictionary<string, object>? dictionary, string errorMessage)
    {
        try
        {
            return JsonSerializer.Serialize(dictionary);
        }
        catch (JsonException)
        {
            throw new ArgumentException(errorMessage);
        }
    }

    public bool ParseBool(bool? boolValue, string errorMessage)
    {
        if (!bool.TryParse(boolValue.ToString(), out var boolResult))
        {
            throw new ArgumentException(errorMessage);
        }

        return boolResult;
    }

    public string ParseString(string? value, string errorMessage)
    {
        var stringValue = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException(errorMessage)
            : value;
        return stringValue;
    }
}