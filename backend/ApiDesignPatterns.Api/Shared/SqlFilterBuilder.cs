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
        for (int i = 0; i < tokens.Count; i++)
        {
            string token = tokens[i];

            // Handle function calls like Email.endsWith("@example.com")
            if (i + 2 < tokens.Count && tokens[i + 1] == "." && _validFunctions.ContainsKey(tokens[i + 2]))
            {
                string field = columnMapper.MapToColumnName(token);
                string function = tokens[i + 2];
                string argument = tokens[i + 3].Trim('"', '(', ')');
                sql.Add(_validFunctions[function](field, argument));
                i += 3; // Skip the next 3 tokens as we've handled them
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
            else if (!token.Contains('.')) // Skip if part of function call
            {
                sql.Add(columnMapper.MapToColumnName(token));
            }
        }

        return string.Join(" ", sql);
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
        { "endsWith", (field, value) => $"{field} LIKE '%{value}'" },
        { "startsWith", (field, value) => $"{field} LIKE '{value}%'" },
        { "contains", (field, value) => $"{field} LIKE '%{value}%'" }
    };

    /// <summary>
    /// Matches quoted strings or individual words.
    /// </summary>
    [GeneratedRegex("\"[^\"]+\"|\\w+(?:\\.\\w+)?(?:\\([^)]*\\))?|\\S+")]
    private static partial Regex FunctionOrQuotedStringOrWordRegex();
}
