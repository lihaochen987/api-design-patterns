using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace backend.Product.Controllers;

public class DynamicContractResolver : DefaultContractResolver
{
    private readonly HashSet<string> _fields;
    private readonly bool _globalWildcard;
    private readonly HashSet<string> _wildcards;

    public DynamicContractResolver(IEnumerable<string> fields)
    {
        _fields = [..fields.Select(f => f.ToLower())];
        _globalWildcard = _fields.Contains("*");
        _wildcards = [.._fields.Where(f => f.EndsWith(".*")).Select(f => f.Split('.')[0])];
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        var declaringTypeName = member.DeclaringType?.Name.ToLower();
        var propertyName = property.PropertyName?.ToLower();

        if (_globalWildcard)
        {
            property.ShouldSerialize = _ => true;
            return property;
        }

        if (declaringTypeName != null && _wildcards.Contains(declaringTypeName))
        {
            property.ShouldSerialize = _ => true;
            return property;
        }

        if (_fields.Contains(propertyName) ||
            (declaringTypeName != null && _fields.Contains($"{declaringTypeName}.{propertyName}")))
        {
            property.ShouldSerialize = _ => true;
        }
        else
        {
            property.ShouldSerialize = _ => false;
        }

        return property;
    }
}