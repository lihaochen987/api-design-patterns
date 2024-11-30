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

    public Dictionary<string, object> ParseStringToDictionary(string? value, string errorMessage)
    {
        var jsonString = JsonSerializer.Serialize(value);
        if (string.IsNullOrWhiteSpace(jsonString))
        {
            throw new ArgumentException(errorMessage);
        }

        try
        {
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
            if (result == null)
            {
                throw new ArgumentException(errorMessage);
            }

            return result;
        }
        catch (JsonException)
        {
            throw new ArgumentException(errorMessage);
        }
    }

    public string ParseDictionaryToString(Dictionary<string, object>? dictionary, string errorMessage)
    {
        if (dictionary == null || dictionary.Count == 0)
        {
            throw new ArgumentException(errorMessage);
        }

        try
        {
            return JsonSerializer.Serialize(dictionary);
        }
        catch (JsonException)
        {
            throw new ArgumentException(errorMessage);
        }
    }
}