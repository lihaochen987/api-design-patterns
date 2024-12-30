namespace backend.Shared.SqlFilter;

/// <summary>
/// SqlFilterBuilder parses a filter string and generates SQL WHERE clauses.
/// </summary>
public abstract class SqlFilterBuilder(ISqlFilterParser filterParser)
{
    /// <summary>
    /// Builds a SQL WHERE clause from the provided filter string.
    /// </summary>
    /// <param name="filter">The filter string to parse (e.g., "Rating > 3 && ProductId == 123").</param>
    /// <returns>A SQL WHERE clause (e.g., "review_rating > 3 AND product_id = 123").</returns>
    public string BuildSqlWhereClause(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return "1=1"; // Default condition for no filters

        List<string> tokens = filterParser.Tokenize(filter);
        return filterParser.GenerateWhereClause(tokens);
    }
}
