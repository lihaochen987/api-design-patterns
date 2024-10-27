using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace backend;

public class DynamicContractResolver(
    IEnumerable<string> fields,
    object targetObject,
    FieldMask fieldMask)
    : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        property.ShouldSerialize = _ => fieldMask.ShouldSerialize(member, fields, targetObject);
        return property;
    }
}