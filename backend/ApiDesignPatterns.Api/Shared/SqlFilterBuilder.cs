using System.Text.RegularExpressions;

namespace backend.Shared;

/// <summary>
/// Builds SQL WHERE clauses by parsing filter strings into valid SQL syntax, using a column mapping strategy.
/// </summary>
/// <remarks>
/// The SqlFilterBuilder supports common comparison operators and logical operators, handling both string
/// and numeric literals while mapping property names to their corresponding database column names.
/// </remarks>
/// <example>
/// Basic usage:
/// <code>
/// IColumnMapper mapper = new ColumnMapper(); // Your implementation
/// var builder = new SqlFilterBuilder(mapper);
///
/// // Simple equals condition
/// string filter1 = "ProductId == 123";
/// string sql1 = builder.BuildSqlWhereClause(filter1);
/// // Result: "product_id = 123"
///
/// // Complex condition with multiple operators
/// string filter2 = "Rating >= 4 &amp;&amp; Price &lt; 100";
/// string sql2 = builder.BuildSqlWhereClause(filter2);
/// // Result: "review_rating >= 4 AND price &lt; 100"
///
/// // String literal handling
/// string filter3 = "Name == \"Test Product\" &amp;&amp; Category != \"Electronics\"";
/// string sql3 = builder.BuildSqlWhereClause(filter3);
/// // Result: "product_name = 'Test Product' AND category &lt;&gt; 'Electronics'"
/// </code>
/// </example>
public partial class SqlFilterBuilder(IColumnMapper columnMapper)
{
    /// <summary>
    /// Builds a SQL WHERE clause by parsing and converting a filter string into valid SQL syntax.
    /// </summary>
    /// <param name="filter">The filter string to parse. Can include comparison operators (==, !=, &lt;, &gt;, &lt;=, &gt;=)
    /// and logical operators (&amp;&amp;, ||). String literals should be enclosed in double quotes.</param>
    /// <returns>A valid SQL WHERE clause string. Returns "1=1" if the filter is null or empty.</returns>
    /// <remarks>
    /// The method handles:
    /// - Property to column name mapping
    /// - Operator conversion
    /// - String and numeric literal formatting
    /// - Multiple conditions joined by logical operators
    /// </remarks>
    /// <example>
    /// <code>
    /// // Empty filter
    /// builder.BuildSqlWhereClause("");
    /// // Output: "1=1"
    ///
    /// // Single condition
    /// builder.BuildSqlWhereClause("Price &lt; 50");
    /// // Output: "price &lt; 50"
    ///
    /// // Multiple conditions'
    /// builder.BuildSqlWhereClause("Category == \"Books\" &amp;&amp; Price &lt; 50 || Rating >= 4");
    /// // Output: "category = 'Books' AND price &lt; 50 OR review_rating &gt;= 4"
    /// </code>
    /// </example>
    public string BuildSqlWhereClause(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return "1=1"; // Default condition for no filters

        List<string> tokens = Tokenize(filter);
        return GenerateWhereClause(tokens);
    }


    /// <summary>
    /// Breaks down a filter string into individual tokens for parsing.
    /// </summary>
    /// <param name="filter">The raw filter string to tokenize.</param>
    /// <returns>A list of tokens representing fields, operators, and values.</returns>
    /// <remarks>
    /// The tokenizer handles:
    /// - Quoted strings (preserving spaces within quotes)
    /// - Operators
    /// - Field names
    /// - Numeric literals
    /// </remarks>
    /// <example>
    /// <code>
    /// // Input: "Name == \"John Doe\" &amp;&amp; Age >= 25"
    /// // Output: ["Name", "==", "\"John Doe\"", "&amp;&amp;", "Age", ">=", "25"]
    ///
    /// // Input: "Price &lt; 100 || Category == \"Books\""
    /// // Output: ["Price", "&lt;", "100", "||", "Category", "==", "\"Books\""]
    /// </code>
    /// </example>
    private List<string> Tokenize(string filter)
    {
        var regex = QuotedStringOrWordRegex();
        var matches = regex.Matches(filter);

        return matches.Select(m => m.Value).ToList();
    }


    /// <summary>
    /// Converts tokenized filter components into a valid SQL WHERE clause.
    /// </summary>
    /// <param name="tokens">The list of tokens generated from the filter string.</param>
    /// <returns>A SQL WHERE clause combining all conditions with proper SQL syntax.</returns>
    /// <remarks>
    /// The generator:
    /// - Maps operators to their SQL equivalents
    /// - Formats string literals with single quotes
    /// - Preserves numeric literals
    /// - Maps property names to database column names
    /// </remarks>
    /// <example>
    /// <code>
    /// // Input tokens: ["Price", "&lt;", "100", "&amp;&amp;", "Category", "==", "\"Books\""]
    /// // Output: "price &lt; 100 AND category = 'Books'"
    ///
    /// // Input tokens: ["Rating", ">=", "4", "||", "IsActive", "==", "true"]
    /// // Output: "review_rating >= 4 OR is_active = true"
    /// </code>
    /// </example>
    private string GenerateWhereClause(List<string> tokens)
    {
        var sql = new List<string>();

        foreach (string token in tokens)
        {
            if (_validOperators.TryGetValue(token, out string? sqlOperator))
            {
                sql.Add(sqlOperator);
            }
            else if (token.StartsWith('"') && token.EndsWith('"'))
            {
                // Handle string literals
                sql.Add($"'{token.Trim('"')}'");
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

    // Dictionary to map logical and comparison operators to SQL syntax
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

    /// <summary>
    /// Generates a regex pattern that matches either quoted strings or individual words.
    /// </summary>
    /// <returns>A Regex object that matches either:
    /// - Quoted strings: Text enclosed in double quotes, containing any characters except quotes
    /// - Words: Continuous sequences of non-whitespace characters</returns>
    /// <remarks>
    /// The regex pattern "\"[^\"]+\"|\\S+" consists of two parts:
    /// 1. \"[^\"]+\" - Matches quoted strings:
    ///    - \" matches a double quote character
    ///    - [^\"]+ matches one or more characters that are not double quotes
    ///    - \" matches the closing double quote
    /// 2. \\S+ - Matches one or more non-whitespace characters
    /// </remarks>
    /// <example>
    /// <code>
    /// var regex = QuotedStringOrWordRegex();
    ///
    /// // Examples of matching:
    /// // Input: "Hello World" && Count > 5
    /// // Matches: ["\"Hello World\"", "&&", "Count", ">", "5"]
    ///
    /// // Input: Name == "John Doe" || Age >= 25
    /// // Matches: ["Name", "==", "\"John Doe\"", "||", "Age", ">=", "25"]
    ///
    /// // Input: Category=="Books & Movies"
    /// // Matches: ["Category", "==", "\"Books & Movies\""]
    /// </code>
    /// </example>
    [GeneratedRegex("\"[^\"]+\"|\\S+")]
    private static partial Regex QuotedStringOrWordRegex();
}
