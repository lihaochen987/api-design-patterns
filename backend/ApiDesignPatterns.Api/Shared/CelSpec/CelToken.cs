namespace backend.Shared.CelSpec;

public enum CelTokenType
{
    Field,
    Operator,
    Value,
    Logical
}

public class CelToken(CelTokenType type, string value)
{
    public CelTokenType Type { get; set; } = type;
    public string Value { get; set; } = value;
}
