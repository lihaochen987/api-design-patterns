// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace backend.Shared.Tests;

public class SqlFilterBuilderTests : SqlFilterBuilderTestBase
{
    [Fact]
    public void BuildSqlWhereClause_EmptyFilter_ReturnsDefaultCondition()
    {
        const string filter = "";
        SqlFilterBuilder sut = SqlFilterBuilder();

        string result = sut.BuildSqlWhereClause(filter);

        Assert.Equal("1=1", result);
    }

    [Fact]
    public void BuildSqlWhereClause_NullFilter_ReturnsDefaultCondition()
    {
        SqlFilterBuilder sut = SqlFilterBuilder();

        string result = sut.BuildSqlWhereClause(null!);

        Assert.Equal("1=1", result);
    }

    [Theory]
    [InlineData("ProductId == 123", "productid = 123")]
    [InlineData("Price >= 100", "price >= 100")]
    [InlineData("Rating < 5", "rating < 5")]
    [InlineData("Status != \"Active\"", "status <> 'Active'")]
    public void BuildSqlWhereClause_BasicOperators_GeneratesCorrectSql(string filter, string expected)
    {
        SqlFilterBuilder sut = SqlFilterBuilder();

        string result = sut.BuildSqlWhereClause(filter);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Rating >= 4 && Price < 100", "rating >= 4 AND price < 100")]
    [InlineData("Status == \"Active\" || Status == \"Pending\"", "status = 'Active' OR status = 'Pending'")]
    public void BuildSqlWhereClause_LogicalOperators_GeneratesCorrectSql(string filter, string expected)
    {
        SqlFilterBuilder sut = SqlFilterBuilder();

        string result = sut.BuildSqlWhereClause(filter);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Email.endsWith(\"@example.com\")", "email LIKE '%@example.com'")]
    [InlineData("Name.startsWith(\"Dr.\")", "name LIKE 'Dr.%'")]
    [InlineData("Description.contains(\"important\")", "description LIKE '%important%'")]
    public void BuildSqlWhereClause_StringFunctions_GeneratesCorrectSql(string filter, string expected)
    {
        SqlFilterBuilder sut = SqlFilterBuilder();

        string result = sut.BuildSqlWhereClause(filter);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BuildSqlWhereClause_ComplexCondition_GeneratesCorrectSql()
    {
        const string filter = "Status == \"Active\" && Rating >= 4 && Email.endsWith(\"@example.com\")";
        const string expected = "status = 'Active' AND rating >= 4 AND email LIKE '%@example.com'";
        SqlFilterBuilder sut = SqlFilterBuilder();

        string result = sut.BuildSqlWhereClause(filter);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BuildSqlWhereClause_QuotedStringsWithSpaces_GeneratesCorrectSql()
    {
        const string filter = "Name == \"John Doe\" && Title == \"Senior Developer\"";
        const string expected = "name = 'John Doe' AND title = 'Senior Developer'";
        SqlFilterBuilder sut = SqlFilterBuilder();

        string result = sut.BuildSqlWhereClause(filter);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BuildSqlWhereClause_EscapesSingleQuotes_GeneratesCorrectSql()
    {
        const string filter = "Name == \"O'Connor\" && Description.contains(\"company's\")";
        const string expected = "name = 'O''Connor' AND description LIKE '%company''s%'";
        SqlFilterBuilder sut = SqlFilterBuilder();

        string result = sut.BuildSqlWhereClause(filter);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("InvalidFunction.notSupported(\"test\")")]
    [InlineData("Email.endsWith")]
    [InlineData("Email.endsWith()")]
    [InlineData(".endsWith(\"test\")")]
    public void BuildSqlWhereClause_InvalidFunctionCalls_IgnoresInvalidParts(string filter)
    {
        SqlFilterBuilder sut = SqlFilterBuilder();
        string result = sut.BuildSqlWhereClause(filter);

        Assert.Equal("", result.Trim());
    }
}
