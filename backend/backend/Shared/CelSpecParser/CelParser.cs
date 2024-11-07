using System.Linq.Expressions;
using System.Reflection;

namespace backend.Shared.CelSpecParser;

/// <summary>
/// CelParser takes in a filter string and translates it into a form that can be used to filter data.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CelParser<T>
{
    private readonly ParameterExpression _parameter = Expression.Parameter(typeof(T), "x");

    // Todo: Refactor this.
    public Expression<Func<T, bool>> ParseFilter(List<CelToken> filterTokens)
    {
        Expression? expression = null;

        for (var i = 0; i < filterTokens.Count; i++)
        {
            var currentToken = filterTokens[i];

            if (currentToken.Type == CelTokenType.Field)
            {
                // Generate property expression and comparison value
                var fieldExpression = GetFieldExpression(currentToken, _parameter);
                var operatorToken = filterTokens[++i];
                var valueToken = filterTokens[++i];
                var comparisonValue = ConvertTokenToExpression(valueToken, fieldExpression.Type);

                // Build comparison expression based on the operator
                var comparisonExpression =
                    BuildComparisonExpression(fieldExpression, operatorToken.Value, comparisonValue);
                expression = expression == null
                    ? comparisonExpression
                    : Expression.AndAlso(expression, comparisonExpression);
            }
            else if (currentToken.Type == CelTokenType.Logical && expression != null)
            {
                var nextExpression = ParseFilter(filterTokens.Skip(i + 1).ToList());
                expression = currentToken.Value == "&&"
                    ? Expression.AndAlso(expression, nextExpression.Body)
                    : Expression.OrElse(expression, nextExpression.Body);
                break;
            }
        }

        return Expression.Lambda<Func<T, bool>>(expression ?? Expression.Constant(true), _parameter);
    }

    /// <summary>
    /// Breaks the filter string into tokens (small meaningful parts)
    /// </summary>
    /// <param name="filterQuery">
    /// "Category == DogFood && Price > 50"
    /// </param>
    /// <returns>
    /// A list of CelTokens would be returned with their value and tagged with the respective type.
    /// Category would be tagged as a Value Field
    /// == would be tagged as an Operator Field
    /// </returns>
    public List<CelToken> Tokenize(string filterQuery)
    {
        Dictionary<string, CelTokenType> operatorsDict = new()
        {
            { "==", CelTokenType.Operator },
            { "!=", CelTokenType.Operator },
            { "<", CelTokenType.Operator },
            { ">", CelTokenType.Operator },
            { "<=", CelTokenType.Operator },
            { ">=", CelTokenType.Operator },
            { "&&", CelTokenType.Logical },
            { "||", CelTokenType.Logical }
        };

        var tokens = new List<CelToken>();
        var parts = filterQuery.Split(' ');

        foreach (var part in parts)
        {
            if (operatorsDict.TryGetValue(part, out var @operator))
                tokens.Add(new CelToken(@operator, part));
            else if (part.StartsWith('\"') && part.EndsWith('\"'))
                tokens.Add(new CelToken(CelTokenType.Value, part.Trim('"')));
            else if (bool.TryParse(part, out _))
                tokens.Add(new CelToken(CelTokenType.Value, part));
            else if (decimal.TryParse(part, out _))
                tokens.Add(new CelToken(CelTokenType.Value, part));
            else
                tokens.Add(new CelToken(CelTokenType.Field, part));
        }

        return tokens;
    }

    private static MemberExpression GetFieldExpression(
        CelToken fieldCelToken,
        ParameterExpression lambdaParameter)
    {
        var property = typeof(T).GetProperty(fieldCelToken.Value,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null)
            throw new ArgumentException($"Property '{fieldCelToken.Value}' does not exist on type '{typeof(T).Name}'");

        var field = Expression.Property(lambdaParameter, property);
        return field;
    }

    /// <summary>
    /// Creates a comparison based on the operator
    /// </summary>
    /// <param name="fieldExpression">
    /// </param>
    /// <param name="comparisonOperator">
    /// </param>
    /// <param name="comparisonValueExpression">
    /// </param>
    /// <returns>
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// </exception>
    private static BinaryExpression BuildComparisonExpression(
        Expression fieldExpression,
        string comparisonOperator,
        Expression comparisonValueExpression)
    {
        return comparisonOperator switch
        {
            "==" => Expression.Equal(fieldExpression, comparisonValueExpression),
            "!=" => Expression.NotEqual(fieldExpression, comparisonValueExpression),
            "<" => Expression.LessThan(fieldExpression, comparisonValueExpression),
            ">" => Expression.GreaterThan(fieldExpression, comparisonValueExpression),
            "<=" => Expression.LessThanOrEqual(fieldExpression, comparisonValueExpression),
            ">=" => Expression.GreaterThanOrEqual(fieldExpression, comparisonValueExpression),
            _ => throw new NotSupportedException($"Operator {comparisonOperator} is not supported")
        };
    }

    /// <summary>
    /// Converts the token to the right format
    /// </summary>
    /// <param name="valueCelToken"></param>
    /// <param name="destinationType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static ConstantExpression ConvertTokenToExpression(
        CelToken valueCelToken,
        Type destinationType)
    {
        var value = valueCelToken.Value switch
        {
            var s when destinationType == typeof(string) => s,
            var s when destinationType == typeof(bool) && bool.TryParse(s, out var boolValue) => boolValue,
            var s when destinationType == typeof(decimal) && decimal.TryParse(s, out var decimalValue) => decimalValue,
            var s when destinationType == typeof(int) && int.TryParse(s, out var intValue) => intValue,
            var s when destinationType.IsEnum && Enum.TryParse(destinationType, s, ignoreCase: true, out var enumValue)
                => enumValue,

            _ => throw new ArgumentException($"Cannot convert '{valueCelToken.Value}' to type '{destinationType.Name}'")
        };

        return Expression.Constant(value, destinationType);
    }
}