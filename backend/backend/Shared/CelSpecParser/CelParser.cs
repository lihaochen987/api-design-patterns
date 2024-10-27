using System.Linq.Expressions;
using System.Reflection;

namespace backend.Shared.CelSpecParser;

public class CelParser<T>
{
    private readonly ParameterExpression _parameter = Expression.Parameter(typeof(T), "x");

    // Todo: Refactor this.
    public Expression<Func<T, bool>> ParseFilter(List<Token> tokens)
    {
        Expression? expression = null;

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (token.Type == TokenType.Field)
            {
                // Generate property expression and comparison value
                var fieldExpression = GetFieldExpression(token, _parameter);
                var operatorToken = tokens[++i];
                var valueToken = tokens[++i];
                var comparisonValue = ConvertTokenToExpression(valueToken, fieldExpression.Type);

                // Build comparison expression based on the operator
                var comparisonExpression =
                    BuildComparisonExpression(fieldExpression, operatorToken.Value, comparisonValue);
                expression = expression == null
                    ? comparisonExpression
                    : Expression.AndAlso(expression, comparisonExpression);
            }
            else if (token.Type == TokenType.Logical && expression != null)
            {
                var nextExpression = ParseFilter(tokens.Skip(i + 1).ToList());
                expression = token.Value == "&&"
                    ? Expression.AndAlso(expression, nextExpression.Body)
                    : Expression.OrElse(expression, nextExpression.Body);
                break;
            }
        }

        return Expression.Lambda<Func<T, bool>>(expression ?? Expression.Constant(true), _parameter);
    }

    public List<Token> Tokenize(string input)
    {
        Dictionary<string, TokenType> operatorsDict = new()
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

    private static Expression GetFieldExpression(Token fieldToken, ParameterExpression parameter)
    {
        var property = typeof(T).GetProperty(fieldToken.Value,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null)
            throw new ArgumentException($"Property '{fieldToken.Value}' does not exist on type '{typeof(T).Name}'");

        var field = Expression.Property(parameter, property);

        return property.PropertyType.IsEnum
            ? Expression.Call(field, typeof(object).GetMethod("ToString", Type.EmptyTypes)!) // For Enum compatability
            : field;
    }

    private static BinaryExpression BuildComparisonExpression(Expression field, string op, Expression value)
    {
        return op switch
        {
            "==" => Expression.Equal(field, value),
            "!=" => Expression.NotEqual(field, value),
            "<" => Expression.LessThan(field, value),
            ">" => Expression.GreaterThan(field, value),
            "<=" => Expression.LessThanOrEqual(field, value),
            ">=" => Expression.GreaterThanOrEqual(field, value),
            _ => throw new NotSupportedException($"Operator {op} is not supported")
        };
    }

    private static ConstantExpression ConvertTokenToExpression(Token valueToken, Type targetType)
    {
        object? value = valueToken.Value switch
        {
            var s when targetType == typeof(string) => s,
            var s when targetType == typeof(bool) && bool.TryParse(s, out var boolValue) => boolValue,
            var s when targetType == typeof(decimal) && decimal.TryParse(s, out var decimalValue) => decimalValue,
            var s when targetType == typeof(int) && int.TryParse(s, out var intValue) => intValue,
            var s when targetType.IsEnum && Enum.TryParse(targetType, s, ignoreCase: true, out var enumValue)
                => enumValue.ToString(),

            _ => throw new ArgumentException($"Cannot convert '{valueToken.Value}' to type '{targetType.Name}'")
        };

        return Expression.Constant(value, targetType.IsEnum ? typeof(string) : targetType);
    }
}