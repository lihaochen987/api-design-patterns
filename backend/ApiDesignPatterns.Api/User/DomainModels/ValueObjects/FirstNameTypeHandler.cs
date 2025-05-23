﻿using backend.Shared.SqlMappers;

namespace backend.User.DomainModels.ValueObjects;

public class FirstNameTypeHandler : StringValueObjectTypeHandler<FirstName>
{
    protected override FirstName Create(string value) => new(value);
    protected override string GetStringValue(FirstName value) => value.ToString();
}
