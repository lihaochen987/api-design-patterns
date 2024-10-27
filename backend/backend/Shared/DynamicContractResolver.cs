using System.Reflection;
using backend.Shared.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace backend.Shared;

public class DynamicContractResolver(
    IEnumerable<string> fields,
    object targetObject,
    ISerializationService serializationService)
    : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        property.ShouldSerialize = _ => serializationService.ShouldSerializeJsonProperty(member, fields, targetObject);
        return property;
    }
}