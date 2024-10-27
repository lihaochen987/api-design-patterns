using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace backend;

public class DynamicContractResolver : DefaultContractResolver
{
    private readonly HashSet<string> _fields;
    private readonly bool _globalWildcard;

    public DynamicContractResolver(
        IEnumerable<string> fields,
        object targetObject)
    {
        _fields = FieldProcessor.PrepareFields(fields, targetObject);
        _globalWildcard = _fields.Contains("*") || _fields.Count == 0;
    }

    protected override JsonProperty CreateProperty(
        MemberInfo member,
        MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (_globalWildcard || _fields.Count == 0)
        {
            property.ShouldSerialize = _ => true;
            return property;
        }

        var propertyName = GetFullPropertyPath(member);
        var propertyNameWithoutPrefix = StringHelper.RemoveContractFromString(
            StringHelper.RemoveResponseFromString(propertyName)).Trim().ToLowerInvariant();

        if (_fields.Contains(propertyNameWithoutPrefix))
        {
            property.ShouldSerialize = _ => true;
            return property;
        }

        if (member is PropertyInfo propertyInfo && TypeHelper.IsComplexType(propertyInfo.PropertyType))
        {
            property.ShouldSerialize = _ =>
                ShouldSerializeWithNestedMatch(propertyInfo, propertyNameWithoutPrefix, _fields);
            return property;
        }

        property.ShouldSerialize = _ => false;
        return property;
    }

    private static bool ShouldSerializeWithNestedMatch(
        PropertyInfo propertyInfo, string parentPath, HashSet<string> fields)
    {
        return propertyInfo.PropertyType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(subProperty => $"{parentPath}.{subProperty.Name.ToLower()}")
            .Any(fields.Contains);
    }

    private static string GetFullPropertyPath(MemberInfo member)
    {
        var path = member.Name;
        var declaringType = member.DeclaringType;

        while (declaringType != null)
        {
            path = declaringType.Name.ToLower() + "." + path;
            declaringType = declaringType.DeclaringType;
        }

        return path;
    }
}