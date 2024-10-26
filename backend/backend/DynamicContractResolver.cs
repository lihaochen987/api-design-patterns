using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace backend;

public partial class DynamicContractResolver : DefaultContractResolver
{
    private readonly HashSet<string> _fields;
    private readonly bool _globalWildcard;
    private readonly HashSet<string> _wildcards;
    private readonly HashSet<string> _availablePaths;

    public DynamicContractResolver(IEnumerable<string> fields, object targetObject)
    {
        var enumerable = fields.ToList();
        _fields = enumerable.Count != 0
            ? [..enumerable.Select(f => f.ToLower())]
            : [];
        _globalWildcard = _fields.Contains("*") || _fields.Count == 0;
        _wildcards = [.._fields.Where(f => f.EndsWith(".*")).Select(f => f.Split('.')[0])];
        _availablePaths = [..FieldMaskHelper.InferFieldMask(targetObject)];
        _fields.RemoveWhere(f => !_availablePaths.Contains(f) && !_wildcards.Contains(f.Split('.')[0]));
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (_globalWildcard || _fields.Count == 0)
        {
            property.ShouldSerialize = _ => true;
            return property;
        }

        var propertyName = GetFullPropertyPath(member);
        var cleanedPropertyName = CleanedPropertyName(propertyName);

        if (_fields.Contains(cleanedPropertyName) && _availablePaths.Contains(cleanedPropertyName))
        {
            property.ShouldSerialize = _ => true;
            return property;
        }

        // Check if this property should be serialized based on nested matches
        if (member is PropertyInfo propertyInfo && IsComplexType(propertyInfo.PropertyType))
        {
            property.ShouldSerialize = _ => ShouldSerializeWithNestedMatch(propertyInfo, propertyName);
            return property;
        }

        property.ShouldSerialize = _ => false;
        return property;
    }

    private static string RemoveResponseClass(string input)
    {
        return RemoveResponseFromString().Replace(input, "").Trim();
    }

    private bool ShouldSerializeWithNestedMatch(PropertyInfo propertyInfo, string parentPath)
    {
        return propertyInfo.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(subProperty => CleanedPropertyName($"{parentPath}.{subProperty.Name.ToLower()}"))
            .Select(RemoveResponseClass).Any(cleanedSubPath =>
                _fields.Contains(cleanedSubPath) && _availablePaths.Contains(cleanedSubPath));
    }

    private static bool IsComplexType(Type type)
    {
        return !type.IsPrimitive && type != typeof(string) && !type.IsEnum;
    }

    private static string CleanedPropertyName(string propertyName)
    {
        return RemoveContractFromString().Replace(propertyName, "").Trim().ToLowerInvariant();
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

    [GeneratedRegex(@"\b\w*response\.\b", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveResponseFromString();

    [GeneratedRegex("contract", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveContractFromString();
}