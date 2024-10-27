using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace backend.Shared;

public class DynamicContractResolver(
    IEnumerable<string> fields,
    object targetObject,
    FieldMaskSerializer fieldMaskSerializer)
    : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        property.ShouldSerialize = _ => fieldMaskSerializer.IsJsonPropertySerializable(member, fields, targetObject);
        return property;
    }
}