using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace backend.Shared.CelSpec;

/// <summary>
/// CelParser takes in a filter string and translates it into a form that can be used to filter data.
/// </summary>
/// <typeparam name="T">The type of object being filtered</typeparam>
public partial class CelParser<T>(TypeParser typeParser)
{
    private readonly ParameterExpression _parameter = Expression.Parameter(typeof(T), "x");

    /// <summary>
    /// Builds a strongly-typed filter expression from a list of CEL tokens.
    /// </summary>
    /// <param name="filterTokens">The tokenized filter expression</param>
    /// <returns>A compiled lambda expression that can be used to filter objects of type T</returns>
    /// <example>
    /// Given tokens representing "Price > 50 && Category == "Food"", creates Expression{Func{T, bool}}
    /// that returns true only for objects matching these criteria
    /// </example>
    public Expression<Func<T, bool>> BuildFilterExpression(List<CelToken> filterTokens)
    {
        Expression? expression = null;

        for (int i = 0; i < filterTokens.Count; i++)
        {
            CelToken currentToken = filterTokens[i];

            if (currentToken.Type == CelTokenType.Field)
            {
                expression = CreateComparisonExpression(filterTokens, ref i, expression, _parameter);
            }
            else if (currentToken.Type == CelTokenType.Logical && expression != null)
            {
                expression = CombineWithLogicalOperator(filterTokens, ref i, expression, currentToken.Value);
                break;
            }
        }

        return Expression.Lambda<Func<T, bool>>(expression ?? Expression.Constant(true), _parameter);
    }

    /// <summary>
    /// Creates a binary comparison expression between a property and a value.
    /// </summary>
    /// <param name="filterTokens">The complete list of filter tokens</param>
    /// <param name="index">Current position in the token list, updated as tokens are consumed</param>
    /// <param name="existingExpression">Optional existing expression to combine with the new comparison</param>
    /// <param name="parameter">The parameter expression representing the object being filtered</param>
    /// <returns>A binary expression representing the comparison</returns>
    /// <example>
    /// For tokens representing "Price > 50", creates Expression equivalent to x => x.Price > 50
    /// </example>
    private BinaryExpression CreateComparisonExpression(
        List<CelToken> filterTokens,
        ref int index,
        Expression? existingExpression,
        ParameterExpression parameter)
    {
        MemberExpression fieldExpression = CreatePropertyAccessExpression(filterTokens[index], parameter);
        CelToken operatorToken = filterTokens[++index];
        CelToken valueToken = filterTokens[++index];
        ConstantExpression comparisonValue = CreateConstantExpression(valueToken, fieldExpression.Type);

        BinaryExpression comparisonExpression =
            CreateComparisonOperator(fieldExpression, operatorToken.Value, comparisonValue);
        return existingExpression == null
            ? comparisonExpression
            : Expression.AndAlso(existingExpression, comparisonExpression);
    }

    /// <summary>
    /// Combines two expressions using a logical operator (AND/OR).
    /// </summary>
    /// <param name="filterTokens">The complete list of filter tokens</param>
    /// <param name="index">Current position in the token list</param>
    /// <param name="existingExpression">The left-hand expression to combine</param>
    /// <param name="logicalOperator">The logical operator ("&amp;&amp;" or "||")</param>
    /// <returns>A binary expression representing the logical combination</returns>
    /// <example>
    /// Given "x > 5" and "y &lt; 10", with "&amp;&amp;" operator, creates "x > 5 AND y &lt; 10"
    /// </example>
    private BinaryExpression CombineWithLogicalOperator(
        List<CelToken> filterTokens,
        ref int index,
        Expression existingExpression,
        string logicalOperator)
    {
        Expression<Func<T, bool>> nextExpression = BuildFilterExpression(filterTokens.Skip(index + 1).ToList());
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

        List<CelToken> tokens = [];

        Regex regex = QuotedStringOrWord();


        foreach (Match match in regex.Matches(filterQuery))
        {
            string part = match.Value;

            if (TryParseOperatorToken(part, operatorsDict, out CelToken? operatorToken))
            {
                if (operatorToken != null)
                {
                    tokens.Add(operatorToken);
                }
            }
            else if (TryParseValueToken(part, out CelToken? valueToken))
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

    /// <summary>
    /// Attempts to parse a string as an operator token.
    /// </summary>
    /// <param name="part">The string to parse</param>
    /// <param name="operatorsDict">Dictionary of valid operators and their types</param>
    /// <param name="token">The resulting token if successful</param>
    /// <returns>True if the string was successfully parsed as an operator, false otherwise</returns>
    private static bool TryParseOperatorToken(
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

    /// <summary>
    /// Attempts to parse a string as a value token (string, boolean, or number).
    /// </summary>
    /// <param name="part">The string to parse</param>
    /// <param name="token">The resulting token if successful</param>
    /// <returns>True if the string was successfully parsed as a value, false otherwise</returns>
    private static bool TryParseValueToken(
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

    /// <summary>
    /// Creates a member access expression for accessing a property path on the filtered object.
    /// </summary>
    /// <param name="fieldCelToken">Token containing the property path</param>
    /// <param name="lambdaParameter">Parameter expression representing the filtered object</param>
    /// <returns>A member expression for accessing the specified property</returns>
    /// <exception cref="ArgumentException">Thrown when a property in the path doesn't exist</exception>
    /// <example>
    /// For token "Product.Price", creates expression equivalent to x => x.Product.Price
    /// </example>
    private static MemberExpression CreatePropertyAccessExpression(
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
    /// Creates a binary comparison operator expression based on the specified operator.
    /// </summary>
    /// <param name="fieldExpression">The left-hand property expression</param>
    /// <param name="comparisonOperator">The string representation of the operator ("==", "!=", etc.)</param>
    /// <param name="comparisonValueExpression">The right-hand value expression</param>
    /// <returns>A binary expression representing the comparison</returns>
    /// <exception cref="NotSupportedException">Thrown when an unsupported operator is encountered</exception>
    /// <example>
    /// Given field "Price" and operator ">", creates expression equivalent to x.Price > [value]
    /// </example>
    private static BinaryExpression CreateComparisonOperator(
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
    /// Creates a typed constant expression from a token value.
    /// </summary>
    /// <param name="valueCelToken">The token containing the value to convert</param>
    /// <param name="destinationType">The target type for the conversion</param>
    /// <returns>A constant expression of the specified type</returns>
    /// <exception cref="ArgumentException">Thrown when the value cannot be converted to the target type</exception>
    /// <example>
    /// Converting "123" to an int constant expression, or "true" to a bool constant expression
    /// </example>
    private ConstantExpression CreateConstantExpression(
        CelToken valueCelToken,
        Type destinationType)
    {
        object value = destinationType switch
        {
            { } t when t == typeof(string) => ConvertToString(valueCelToken.Value),
            { } t when t == typeof(bool) => ConvertToBool(valueCelToken.Value),
            { } t when t == typeof(decimal) => typeParser.ParseDecimal(valueCelToken.Value, "Invalid Decimal Value"),
            { } t when t == typeof(int) => ConvertToInt(valueCelToken.Value),
            { } t when t == typeof(long) => typeParser.ParseLong(valueCelToken.Value, "Invalid Long Value"),
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
