namespace backend.Shared.CelSpecParser;

public class CelOperators
{
    public readonly Dictionary<string, TokenType> CelOperatorsDict = new()
    {
        { "==", TokenType.Operator },
        { "!=", TokenType.Operator },
        { "<", TokenType.Operator },
        { ">", TokenType.Operator },
        { "<=", TokenType.Operator },
        { ">=", TokenType.Operator },
        { "&&", TokenType.Logical },
        { "||", TokenType.Logical }
    };
}