using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace backend;

public class DynamicContractResolver : DefaultContractResolver
{
    private readonly HashSet<string> _fields;
    private readonly bool _globalWildcard;
    private readonly HashSet<string> _wildcards;
    private readonly HashSet<string> _availablePaths;

    public DynamicContractResolver(IEnumerable<string> fields, object targetObject)
    {
        _fields = [..fields.Select(f => f.ToLower())];
        _globalWildcard = _fields.Contains("*") || _fields.Count == 0;
        _wildcards = [.._fields.Where(f => f.EndsWith(".*")).Select(f => f.Split('.')[0])];
        _availablePaths = [..FieldMaskHelper.InferFieldMask(targetObject)];
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (_globalWildcard)
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
    
    public static string RemoveGetProductResponse(string input)
    {
        // Use a case-insensitive regex to match "getproductresponse" anywhere in the string
        var pattern = @"getproductresponse.";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);

        // Replace "getproductresponse" with an empty string if it exists
        return regex.Replace(input, "").Trim();
    }

    private bool ShouldSerializeWithNestedMatch(PropertyInfo propertyInfo, string parentPath)
    {
        // Iterate through each sub-property of the complex type
        foreach (var subProperty in
                 propertyInfo.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // Construct the full path of the sub-property
            var subPath = CleanedPropertyName($"{parentPath}.{subProperty.Name.ToLower()}");
            var cleanedSubPath = RemoveGetProductResponse(subPath);

            // Check if the full path of this sub-property is in both _fields and _availablePaths
            if (_fields.Contains(cleanedSubPath) && _availablePaths.Contains(cleanedSubPath))
            {
                return true; // Serialize the parent property if any sub-property matches
            }
        }

        return false; // No matching sub-property found, do not serialize the parent property
    }

    private bool IsComplexType(Type type)
    {
        return !type.IsPrimitive && type != typeof(string) && !type.IsEnum;
    }

    private string CleanedPropertyName(string propertyName)
    {
        return Regex.Replace(propertyName, "contract", "", RegexOptions.IgnoreCase).Trim().ToLowerInvariant();
    }

    private string GetFullPropertyPath(MemberInfo member)
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