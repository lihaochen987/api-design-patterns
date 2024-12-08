using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace backend.Shared.CelSpec;

/// <summary>
///     CelParser takes in a filter string and translates it into a form that can be used to filter data.
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class CelParser<T>(TypeParser typeParser)
{
    private readonly ParameterExpression _parameter = Expression.Parameter(typeof(T), "x");

    public Expression<Func<T, bool>> ParseFilter(List<CelToken> filterTokens)
    {
        Expression? expression = null;

        for (int i = 0; i < filterTokens.Count; i++)
        {
            CelToken currentToken = filterTokens[i];

            if (currentToken.Type == CelTokenType.Field)
            {
                expression = BuildFieldExpression(filterTokens, ref i, expression, _parameter);
            }
            else if (currentToken.Type == CelTokenType.Logical && expression != null)
            {
                expression = BuildLogicalExpression(filterTokens, ref i, expression, currentToken.Value);
                break;
            }
        }

        return Expression.Lambda<Func<T, bool>>(expression ?? Expression.Constant(true), _parameter);
    }

    private BinaryExpression BuildFieldExpression(
        List<CelToken> filterTokens,
        ref int index,
        Expression? existingExpression,
        ParameterExpression parameter)
    {
        MemberExpression fieldExpression = GetFieldExpression(filterTokens[index], parameter);
        CelToken operatorToken = filterTokens[++index];
        CelToken valueToken = filterTokens[++index];
        ConstantExpression comparisonValue = ConvertTokenToExpression(valueToken, fieldExpression.Type);

        BinaryExpression comparisonExpression =
            BuildComparisonExpression(fieldExpression, operatorToken.Value, comparisonValue);
        return existingExpression == null
            ? comparisonExpression
            : Expression.AndAlso(existingExpression, comparisonExpression);
    }

    private BinaryExpression BuildLogicalExpression(
        List<CelToken> filterTokens,
        ref int index,
        Expression existingExpression,
        string logicalOperator)
    {
        Expression<Func<T, bool>> nextExpression = ParseFilter(filterTokens.Skip(index + 1).ToList());
        return logicalOperator == "&&"
            ? Expression.AndAlso(existingExpression, nextExpression.Body)
            : Expression.OrElse(existingExpression, nextExpression.Body);
    }

    /// <summary>
    ///     Breaks the filter string into tokens (small meaningful parts)
    /// </summary>
    /// <param name="filterQuery">
    ///     "Category == DogFood && Price > 50"
    /// </param>
    /// <returns>
    ///     A list of CelTokens would be returned with their value and tagged with the respective type.
    ///     Category would be tagged as a Value Field
    ///     == would be tagged as an Operator Field
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

        List<CelToken> tokens = new();

        Regex regex = QuotedStringOrWord();


        foreach (Match match in regex.Matches(filterQuery))
        {
            string part = match.Value;

            if (TryGetOperatorToken(part, operatorsDict, out CelToken? operatorToken))
            {
                if (operatorToken != null)
                {
                    tokens.Add(operatorToken);
                }
            }
            else if (TryGetValueToken(part, out CelToken? valueToken))
            {
                if (valueToken != null)
                {
                    tokens.Add(valueToken);
                }
            }
            else
            {
                tokens.Add(new CelToken(CelTokenType.Field, part));
            }
        }

        return tokens;
    }

    private static bool TryGetOperatorToken(
        string part,
        Dictionary<string, CelTokenType> operatorsDict,
        out CelToken? token)
    {
        if (operatorsDict.TryGetValue(part, out CelTokenType @operator))
        {
            token = new CelToken(@operator, part);
            return true;
        }

        token = null;
        return false;
    }

    private static bool TryGetValueToken(
        string part,
        out CelToken? token)
    {
        if (part.StartsWith('\"') && part.EndsWith('\"'))
        {
            token = new CelToken(CelTokenType.Value, part.Trim('"'));
            return true;
        }

        if (bool.TryParse(part, out _))
        {
            token = new CelToken(CelTokenType.Value, part);
            return true;
        }

        if (decimal.TryParse(part, out _))
        {
            token = new CelToken(CelTokenType.Value, part);
            return true;
        }

        token = null;
        return false;
    }

    private static MemberExpression GetFieldExpression(
        CelToken fieldCelToken,
        ParameterExpression lambdaParameter)
    {
        string[] propertyPath = fieldCelToken.Value.Split('.');

        Expression currentExpression = lambdaParameter;

        foreach (string propertyName in propertyPath)
        {
            PropertyInfo? propertyInfo = currentExpression.Type.GetProperty(propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new ArgumentException(
                    $"Property '{propertyName}' does not exist on type '{currentExpression.Type.Name}'");
            }

            currentExpression = Expression.Property(currentExpression, propertyInfo);
        }

        return (MemberExpression)currentExpression;
    }

    /// <summary>
    ///     Creates a comparison based on the operator
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
        Expression comparisonValueExpression) =>
        comparisonOperator switch
        {
            "==" => Expression.Equal(fieldExpression, comparisonValueExpression),
            "!=" => Expression.NotEqual(fieldExpression, comparisonValueExpression),
            "<" => Expression.LessThan(fieldExpression, comparisonValueExpression),
            ">" => Expression.GreaterThan(fieldExpression, comparisonValueExpression),
            "<=" => Expression.LessThanOrEqual(fieldExpression, comparisonValueExpression),
            ">=" => Expression.GreaterThanOrEqual(fieldExpression, comparisonValueExpression),
            _ => throw new NotSupportedException($"Operator {comparisonOperator} is not supported")
        };

    /// <summary>
    ///     Converts the token to the right format
    /// </summary>
    /// <param name="valueCelToken"></param>
    /// <param name="destinationType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private ConstantExpression ConvertTokenToExpression(
        CelToken valueCelToken,
        Type destinationType)
    {
        object value = destinationType switch
        {
            { } t when t == typeof(string) => ConvertToString(valueCelToken.Value),
            { } t when t == typeof(bool) => ConvertToBool(valueCelToken.Value),
            { } t when t == typeof(decimal) => typeParser.ParseDecimal(valueCelToken.Value, "Invalid Decimal Value"),
            { } t when t == typeof(int) => ConvertToInt(valueCelToken.Value),
            { IsEnum: true } => ConvertToEnum(destinationType, valueCelToken.Value),
            _ => throw new ArgumentException($"Cannot convert '{valueCelToken.Value}' to type '{destinationType.Name}'")
        };

        return Expression.Constant(value, destinationType);
    }

    private static string ConvertToString(string value) => value;

    private static object ConvertToBool(string value) =>
        bool.TryParse(value, out bool boolValue) ? boolValue : throw new ArgumentException("Invalid boolean value");

    private static object ConvertToInt(string value) =>
        int.TryParse(value, out int intValue) ? intValue : throw new ArgumentException("Invalid integer value");

    private static object ConvertToEnum(Type enumType, string value) =>
        Enum.TryParse(enumType, value, true, out object? enumValue)
            ? enumValue
            : throw new ArgumentException("Invalid enum value");

    [GeneratedRegex("\"[^\"]+\"|\\S+")]
    private static partial Regex QuotedStringOrWord();
}
