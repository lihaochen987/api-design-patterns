using backend.Shared.SqlMappers;

namespace backend.Supplier.DomainModels.ValueObjects;

public class AreaCodeTypeHandler : StringValueObjectTypeHandler<AreaCode>
{
    protected override AreaCode Create(string value) => new(value);
    protected override string GetStringValue(AreaCode value) => value.ToString();
}
