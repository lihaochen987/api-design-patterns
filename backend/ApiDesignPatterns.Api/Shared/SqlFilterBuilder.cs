using System.Text.RegularExpressions;

namespace backend.Shared;

/// <summary>
/// SqlFilterBuilder parses a filter string and generates SQL WHERE clauses.
/// </summary>
public abstract partial class SqlFilterBuilder(IColumnMapper columnMapper)
{
    // Dictionary to map logical and comparison operators to SQL syntax
    private static readonly Dictionary<string, string> s_operators = new()
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

    /// <summary>
    /// Builds a SQL WHERE clause from the provided filter string.
    /// </summary>
    /// <param name="filter">The filter string to parse (e.g., "Rating > 3 && ProductId == 123").</param>
    /// <returns>A SQL WHERE clause (e.g., "review_rating > 3 AND product_id = 123").</returns>
    public string BuildSqlWhereClause(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return "1=1"; // Default condition for no filters

        List<string> tokens = Tokenize(filter);
        return GenerateWhereClause(tokens, columnMapper);
    }

    /// <summary>
    /// Tokenizes the filter string into parts (fields, operators, and values).
    /// </summary>
    /// <param name="filter">The raw filter string.</param>
    /// <returns>A list of tokens representing the filter components.</returns>
    private static List<string> Tokenize(string filter)
    {
        // Match quoted strings, words, and operators
        var regex = QuotedStringOrWordRegex();
        var matches = regex.Matches(filter);

        return matches.Select(m => m.Value).ToList();
    }

    /// <summary>
    /// Generates the SQL WHERE clause from tokens.
    /// </summary>
    /// <param name="tokens">The list of tokens from the filter string.</param>
    /// <param name="columnMapper">The column mapper interface which is overriden to map to different column names for
    /// different properties</param>
    /// <returns>The SQL WHERE clause as a string.</returns>
    private static string GenerateWhereClause(List<string> tokens, IColumnMapper columnMapper)
    {
        var sql = new List<string>();

        foreach (string token in tokens)
        {
            if (s_operators.TryGetValue(token, out string? sqlOperator))
            {
                sql.Add(sqlOperator);
            }
            else if (token.StartsWith('\"') && token.EndsWith('\"'))
            {
                // Handle string literals
                sql.Add($"'{token.Trim('\"')}'");
            }
            else if (decimal.TryParse(token, out _))
            {
                // Handle numeric literals
                sql.Add(token);
            }
            else
            {
                // Map property name to column name
                string columnName = columnMapper.MapToColumnName(token);
                sql.Add(columnName);
            }
        }

        return string.Join(" ", sql);
    }

    [GeneratedRegex("\"[^\"]+\"|\\S+")]
    private static partial Regex QuotedStringOrWordRegex();
}
