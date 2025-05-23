﻿// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.SqlMappers;

namespace backend.User.DomainModels.ValueObjects;

public class LastNameTypeHandler : StringValueObjectTypeHandler<LastName>
{
    protected override LastName Create(string value) => new(value);
    protected override string GetStringValue(LastName value) => value.ToString();
}
