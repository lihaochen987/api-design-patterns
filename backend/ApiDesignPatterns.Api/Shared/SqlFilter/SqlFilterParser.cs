// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;

namespace backend.Shared.SqlFilter;

/// <summary>
/// SqlFilterParser is responsible for parsing filter strings into SQL tokens and clauses.
/// </summary>
public partial class SqlFilterParser(
    IColumnMapper columnMapper)
{
    /// <summary>
    /// Tokenizes the filter string into parts (fields, operators, and values).
    /// </summary>
    /// <param name="filter">The raw filter string.</param>
    /// <returns>A list of tokens representing the filter components.</returns>
    public List<string> Tokenize(string filter)
    {
        var regex = QuotedStringOrWordRegex();
        var matches = regex.Matches(filter);

        return matches.Select(m => m.Value).ToList();
    }

    /// <summary>
    /// Generates the SQL WHERE clause from tokens.
    /// </summary>
    /// <param name="tokens">The list of tokens from the filter string.</param>
    /// <returns>The SQL WHERE clause as a string.</returns>
    public string GenerateWhereClause(List<string> tokens)
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

    [GeneratedRegex("\"[^\"]+\"|\\S+")]
    private static partial Regex QuotedStringOrWordRegex();
}
