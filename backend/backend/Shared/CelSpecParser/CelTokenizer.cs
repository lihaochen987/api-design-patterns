namespace backend.Shared.CelSpecParser;

public class CelTokenizer
{
    public List<Token> Tokenize(string input, Dictionary<string, TokenType> operatorsDict)
    {
        var tokens = new List<Token>();
        var parts = input.Split(' ');

        foreach (var part in parts)
        {
            if (operatorsDict.TryGetValue(part, out var @operator))
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