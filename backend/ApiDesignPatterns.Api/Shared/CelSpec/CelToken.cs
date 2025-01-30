namespace backend.Shared.CelSpec;

/// <summary>
/// Represents the different types of tokens in a CEL (Common Expression Language) expression.
/// </summary>
/// <remarks>
/// These tokens are used to break down filter expressions into meaningful parts that can be
/// processed by the CelParser.
/// </remarks>
public enum CelTokenType
{
    /// <summary>
    /// Represents a field or property name in the expression.
    /// Example: In "Price > 100", "Price" is a Field token.
    /// </summary>
    Field,

    /// <summary>
    /// Represents a comparison operator.
    /// Examples: ==, !=, &gt;, &lt;, &gt;=, &lt;=
    /// </summary>
    Operator,

    /// <summary>
    /// Represents a literal value in the expression.
    /// Examples: In "Category == 'Food'", "'Food'" is a Value token.
    /// </summary>
    Value,

    /// <summary>
    /// Represents a logical operator that combines conditions.
    /// Examples: &&, ||
    /// </summary>
    Logical
}

/// <summary>
/// Represents a token in a CEL expression, containing both its type and value.
/// </summary>
/// <param name="type">The type of the token</param>
/// <param name="value">The string value of the token</param>
/// <remarks>
/// CelTokens are created during the tokenization phase of parsing a CEL expression
/// and are used to build the final expression tree.
/// </remarks>
/// <example>
/// Here's an example of how tokens represent a filter expression:
/// <code>
/// // For the filter: "Price > 100 &amp;&amp; Category == 'Food'"
/// var tokens = new List&lt;CelToken&gt;
/// {
///     new CelToken(CelTokenType.Field, "Price"),
///     new CelToken(CelTokenType.Operator, ">"),
///     new CelToken(CelTokenType.Value, "100"),
///     new CelToken(CelTokenType.Logical, "&amp;&amp;"),
///     new CelToken(CelTokenType.Field, "Category"),
///     new CelToken(CelTokenType.Operator, "=="),
///     new CelToken(CelTokenType.Value, "Food")
/// };
/// </code>
/// </example>
public class CelToken(CelTokenType type, string value)
{
    /// <summary>
    /// Gets or sets the type of the token.
    /// </summary>
    public CelTokenType Type { get; set; } = type;

    /// <summary>
    /// Gets or sets the string value of the token.
    /// </summary>
    /// <remarks>
    /// For Field tokens: The property name
    /// For Operator tokens: The operator symbol
    /// For Value tokens: The literal value as a string
    /// For Logical tokens: The logical operator symbol
    /// </remarks>
    public string Value { get; set; } = value;
}
