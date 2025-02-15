// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Moq;

namespace backend.Shared.Tests;

public abstract class SqlFilterBuilderTestBase
{
    protected readonly Mock<IColumnMapper> MockMapper;
    private readonly SqlFilterBuilder _builder;

    protected SqlFilterBuilderTestBase()
    {
        MockMapper = new Mock<IColumnMapper>();
        MockMapper.Setup(m => m.MapToColumnName(It.IsAny<string>()))
            .Returns<string>(s => s.ToLower());
        _builder = new SqlFilterBuilder(MockMapper.Object);
    }

    protected SqlFilterBuilder SqlFilterBuilder()
    {
        return _builder;
    }
}
