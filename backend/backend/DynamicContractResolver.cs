using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace backend;

public partial class DynamicContractResolver : DefaultContractResolver
{
    private readonly HashSet<string> _fields;
    private readonly bool _globalWildcard;
    private readonly HashSet<string> _availablePaths;

    // Todo: Refactor this
    public DynamicContractResolver(IEnumerable<string> fields, object targetObject)
    {
        var enumerable = fields.ToList();
        _fields = enumerable.Count != 0
            ? [..enumerable.Select(f => f.ToLower())]
            : [];
        _globalWildcard = _fields.Contains("*") || _fields.Count == 0;
        var wildcards = _fields.Where(f => f.EndsWith(".*")).Select(f => f.Split('.')[0]).ToList();
        if (wildcards.Count != 0)
        {
            foreach (var wildcard in wildcards)
            {
                AddSubPropertiesToFields(wildcard, targetObject);
            }
        }

        _availablePaths = [..FieldMaskHelper.InferFieldMask(targetObject)];
        _fields.RemoveWhere(f => !_availablePaths.Contains(f));
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
        var cleanedPropertyName = RemoveContractFromString().Replace(propertyName, "").Trim().ToLowerInvariant();

        if (_fields.Contains(cleanedPropertyName) && _availablePaths.Contains(cleanedPropertyName))
        {
            property.ShouldSerialize = _ => true;
            return property;
        }

        if (member is PropertyInfo propertyInfo && IsComplexType(propertyInfo.PropertyType))
        {
            property.ShouldSerialize = _ => ShouldSerializeWithNestedMatch(propertyInfo, cleanedPropertyName);
            return property;
        }

        property.ShouldSerialize = _ => false;
        return property;
    }

    private void AddSubPropertiesToFields(string rootProperty, object targetObject)
    {
        var camelCaseRootProperty = ToCamelCase(rootProperty);
        var rootPropertyInfo =
            targetObject.GetType().GetProperty(camelCaseRootProperty, BindingFlags.Public | BindingFlags.Instance);

        if (rootPropertyInfo == null || !IsComplexType(rootPropertyInfo.PropertyType)) return;
        var subProperties =
            rootPropertyInfo.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var subProperty in subProperties)
        {
            var subPropertyPath = $"{rootProperty}.{subProperty.Name.ToLower()}";
            _fields.Add(subPropertyPath);
        }
    }

    private static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsUpper(input[0]))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1);
    }

    private bool ShouldSerializeWithNestedMatch(PropertyInfo propertyInfo, string parentPath)
    {
        // Include sub-properties if they exist in _fields or match any path in _wildcards
        return propertyInfo.PropertyType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(subProperty => $"{parentPath}.{subProperty.Name.ToLower()}")
            .Any(subPropertyPath => _fields.Contains(subPropertyPath));
    }

    private static bool IsComplexType(Type type)
    {
        return !type.IsPrimitive && type != typeof(string) && !type.IsEnum;
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

        return RemoveResponseFromString().Replace(path, "").Trim();
    }

    [GeneratedRegex(@"\b\w*response\.\b", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveResponseFromString();

    [GeneratedRegex("contract", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveContractFromString();
}