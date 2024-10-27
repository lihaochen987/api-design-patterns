namespace backend.Parsers;

public class CelTokenizer
{
    private readonly Dictionary<string, TokenType> _operators = new()
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

    public List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        var parts = input.Split(' ');

        foreach (var part in parts)
        {
            if (_operators.TryGetValue(part, out var @operator))
                tokens.Add(new Token(@operator, part));
            else if (part.StartsWith('\"') && part.EndsWith('\"'))
                tokens.Add(new Token(TokenType.Value, part.Trim('"')));
            else if (bool.TryParse(part, out _))
                tokens.Add(new Token(TokenType.Value, part));
            else if (decimal.TryParse(part, out _))
                tokens.Add(new Token(TokenType.Value, part));
            else
                tokens.Add(new Token(TokenType.Field, part));
        }

        return tokens;
    }
}