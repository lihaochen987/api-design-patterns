using System.Text.RegularExpressions;

namespace backend.Shared;

/// <summary>
/// Builds SQL WHERE clauses from filter strings using column mapping.
/// </summary>
/// <example>
/// var builder = new SqlFilterBuilder(mapper);
///
/// // Basic filters'
/// builder.BuildSqlWhereClause("ProductId == 123");  // "product_id = 123"
/// builder.BuildSqlWhereClause("Rating >= 4 &amp;&amp; Price &lt; 100");  // "review_rating >= 4 AND price &lt; 100"
/// builder.BuildSqlWhereClause("Name == \"Test Product\"");  // "product_name = 'Test Product'"
/// </example>
public partial class SqlFilterBuilder(IColumnMapper columnMapper)
{
    /// <summary>
    /// Converts a filter string into a SQL WHERE clause.
    /// </summary>
    /// <param name="filter">Filter string with operators (==, !=, &lt;, >, &lt;=, >=, &amp;&amp;, ||)</param>
    /// <returns>SQL WHERE clause or "1=1" if filter is empty</returns>
    public string BuildSqlWhereClause(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return "1=1"; // Default condition for no filters

        List<string> tokens = Tokenize(filter);
        return GenerateWhereClause(tokens);
    }

    /// <summary>
    /// Tokenizes a filter string into individual components.
    /// </summary>
    private static List<string> Tokenize(string filter)
    {
        var regex = FunctionOrQuotedStringOrWordRegex();
        var matches = regex.Matches(filter);

        return matches.Select(m => m.Value).ToList();
    }

    /// <summary>
    /// Converts tokens into a SQL WHERE clause.
    /// </summary>
    private string GenerateWhereClause(List<string> tokens)
    {
        var sql = new List<string>();
        foreach (string token in tokens)
        {
            string? functionResult = TryProcessCompleteFunction(token);
            if (functionResult != null)
            {
                sql.Add(functionResult);
                continue;
            }

            if (_validOperators.TryGetValue(token, out string? sqlOperator))
            {
                sql.Add(sqlOperator);
            }
            else if (token.StartsWith('"') && token.EndsWith('"'))
            {
                sql.Add($"'{token.Trim('"')}'");
            }
            else if (decimal.TryParse(token, out _))
            {
                sql.Add(token);
            }
            else if (!token.Contains('.'))
            {
                sql.Add(columnMapper.MapToColumnName(token));
            }
        }

        return string.Join(" ", sql);
    }

    private string? TryProcessCompleteFunction(string token)
    {
        // Match pattern: field.functionName("value")
        var functionRegex = FunctionRegex();
        var match = functionRegex.Match(token);

        if (!match.Success)
        {
            return null;
        }

        string field = match.Groups[1].Value;
        string functionName = match.Groups[2].Value.ToLower();
        string argument = match.Groups[3].Value;

        if (!_validFunctions.TryGetValue(functionName, out var formatter))
        {
            return null;
        }

        string mappedField = columnMapper.MapToColumnName(field);
        string escapedArgument = argument.Replace("'", "''");
        return formatter(mappedField, escapedArgument);
    }

    private readonly Dictionary<string, string> _validOperators = new()
    {
        { "==", "=" },
        { "!=", "<>" },
        { "<", "<" },
        { ">", ">" },
        { "<=", "<=" },
        { ">=", ">=" },
        { "&&", "AND" },
        { "||", "OR" }
    };

    private readonly Dictionary<string, Func<string, string, string>> _validFunctions = new()
    {
        { "endswith", (field, value) => $"{field} LIKE '%{value}'" },
        { "startswith", (field, value) => $"{field} LIKE '{value}%'" },
        { "contains", (field, value) => $"{field} LIKE '%{value}%'" }
    };

    /// <summary>
    /// Matches quoted strings or individual words.
    /// </summary>
    [GeneratedRegex("\"[^\"]+\"|\\w+(?:\\.\\w+)?(?:\\([^)]*\\))?|\\S+")]
    private static partial Regex FunctionOrQuotedStringOrWordRegex();

    [GeneratedRegex("""^(\w+)\.(\w+)\("([^"]+)"\)$""")]
    private static partial Regex FunctionRegex();
}
