using backend.Shared.SqlMappers;

namespace backend.PhoneNumber.DomainModels.ValueObjects;

public class AreaCodeTypeHandler : StringValueObjectTypeHandler<AreaCode>
{
    protected override AreaCode Create(string value) => new(value);
    protected override string GetStringValue(AreaCode value) => value.ToString();
}
