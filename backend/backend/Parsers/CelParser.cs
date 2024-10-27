using System.Linq.Expressions;
using System.Reflection;

namespace backend.Parsers;

public class CelParser<T>
{
    private readonly ParameterExpression _parameter = Expression.Parameter(typeof(T), "x");

    public Expression<Func<T, bool>> ParseFilter(List<Token> tokens)
    {
        Expression? expression = null;

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (token.Type == TokenType.Field)
            {
                var property = typeof(T).GetProperty(token.Value,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    throw new ArgumentException($"Property '{token.Value}' does not exist on type '{typeof(T).Name}'");

                // Generate property expression
                var field = Expression.Property(_parameter, property);
                Expression fieldForComparison = field;

                // If the property is an enum, convert to string for comparison (assuming database stores as string)
                if (property.PropertyType.IsEnum)
                {
                    fieldForComparison = Expression.Call(field,
                        property.PropertyType.GetMethod("ToString", Type.EmptyTypes)!);
                }

                var op = tokens[++i];
                var valueToken = tokens[++i];

                // Convert the token to the target type
                Expression value = ConvertTokenToExpression(valueToken, property.PropertyType);

                // Create comparison expression
                Expression comparison = op.Value switch
                {
                    "==" => Expression.Equal(fieldForComparison, value),
                    "!=" => Expression.NotEqual(fieldForComparison, value),
                    "<" => Expression.LessThan(fieldForComparison, value),
                    ">" => Expression.GreaterThan(fieldForComparison, value),
                    "<=" => Expression.LessThanOrEqual(fieldForComparison, value),
                    ">=" => Expression.GreaterThanOrEqual(fieldForComparison, value),
                    _ => throw new NotSupportedException($"Operator {op.Value} is not supported")
                };

                expression = expression == null ? comparison : Expression.AndAlso(expression, comparison);
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

        // Return a string constant for enums or the actual type for other properties
        return Expression.Constant(value, targetType.IsEnum ? typeof(string) : targetType);
    }
}