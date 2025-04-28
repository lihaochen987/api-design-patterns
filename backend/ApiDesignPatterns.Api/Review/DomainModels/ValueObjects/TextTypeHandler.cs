using backend.Shared.SqlMappers;

namespace backend.Review.DomainModels.ValueObjects;

public class TextTypeHandler : StringValueObjectTypeHandler<Text>
{
    protected override Text Create(string value) => new(value);
    protected override string GetStringValue(Text value) => value.ToString();
}
