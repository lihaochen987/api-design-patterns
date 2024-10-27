namespace backend.Parsers;

public enum TokenType
{
    Field,
    Operator,
    Value,
    Logical
}

public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }
}