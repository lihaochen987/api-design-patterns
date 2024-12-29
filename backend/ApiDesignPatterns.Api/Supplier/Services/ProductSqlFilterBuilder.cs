using System.Text.RegularExpressions;

namespace backend.Supplier.Services;

/// <summary>
/// SqlFilterBuilder parses a filter string and generates SQL WHERE clauses.
/// </summary>
public partial class ProductSqlFilterBuilder
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
        return GenerateWhereClause(tokens);
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
    /// <returns>The SQL WHERE clause as a string.</returns>
    private static string GenerateWhereClause(List<string> tokens)
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
                string columnName = MapToColumnName(token);
                sql.Add(columnName);
            }
        }

        return string.Join(" ", sql);
    }

    /// <summary>
    /// Maps a property name to its corresponding database column name.
    /// </summary>
    /// <param name="propertyName">The property name (e.g., "Rating").</param>
    /// <returns>The corresponding database column name (e.g., "review_rating").</returns>
    private static string MapToColumnName(string propertyName)
    {
        return propertyName switch
        {
            "Id" => "supplier_id",
            "FullName" => "supplier_fullname",
            "Email" => "supplier_email",
            "CreatedAt" => "supplier_created_at",
            "Street" => "supplier_address_street",
            "City" => "supplier_address_city",
            "PostalCode" => "supplier_address_postal_code",
            "Country" => "supplier_address_country",
            "PhoneNumber" => "supplier_phone_number",
            _ => throw new ArgumentException($"Invalid property name: {propertyName}")
        };
    }

    [GeneratedRegex("\"[^\"]+\"|\\S+")]
    private static partial Regex QuotedStringOrWordRegex();
}
