namespace backend.Product.DomainModels;

public record TaxRate
{
    private readonly decimal _value;

    private TaxRate()
    {
    }

    public TaxRate(decimal value)
    {
        if (value is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "Discount percentage must be between 0 and 100.");

        _value = value;
    }

    public static bool IsValid(decimal candidate)
    {
        return candidate is >= 0 and <= 100;
    }

    public static bool TryParse(string candidate, out TaxRate? taxRate)
    {
        taxRate = null;
        if (!decimal.TryParse(candidate, out var result))
            return false;

        if (!IsValid(result))
            return false;

        taxRate = new TaxRate(result);
        return true;
    }

    public static implicit operator decimal(TaxRate taxRate)
    {
        return taxRate._value;
    }
    
    public override string ToString()
    {
        return $"{_value:0.##}";
    }
}