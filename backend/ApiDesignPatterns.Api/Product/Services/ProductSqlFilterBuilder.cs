// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.SqlFilter;

namespace backend.Product.Services;

public class ProductSqlFilterBuilder(SqlFilterParser filterParser) : SqlFilterBuilder(filterParser);
