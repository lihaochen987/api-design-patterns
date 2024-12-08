namespace backend.Shared.CelSpec;

public enum CelTokenType
{
    Field,
    Operator,
    Value,
    Logical
}

public class CelToken
{
    public CelToken(CelTokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public CelTokenType Type { get; set; }
    public string Value { get; set; }
}
