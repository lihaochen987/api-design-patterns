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
}