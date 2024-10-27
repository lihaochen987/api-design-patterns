using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using backend.Shared.Utilities;

namespace backend.Shared.FieldMasks;

public partial class FieldMaskSerializer
{
    public bool IsJsonPropertySerializable(
        MemberInfo member,
        IEnumerable<string> fieldMask,
        object entityToSerialize)
    {
        var fields = GetValidFields(fieldMask, entityToSerialize);

        // Handle global wildcard or empty field mask as serializable
        if (fields.Contains("*") || fields.Count == 0)
        {
            return true;
        }

        if (fields.Contains(member.Name.ToLowerInvariant()))
        {
            return true;
        }

        var propertyName = BuildFullPath(member);
        var propertyNameWithoutPrefix = RemoveRegex(
            propertyName,
            RemoveResponseRegex(),
            RemoveContractRegex(),
            RemoveRequestRegex()).ToLowerInvariant();

        // Check if the exact property name exists in the field mask
        if (fields.Contains(propertyNameWithoutPrefix))
        {
            return true;
        }

        // If the property is complex, check if any sub-properties are in the field mask
        if (member is PropertyInfo propertyInfo && TypeDetector.IsComplexType(propertyInfo.PropertyType))
        {
            return propertyInfo.PropertyType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(subProperty => $"{propertyNameWithoutPrefix}.{subProperty.Name.ToLower()}")
                .Any(fields.Contains);
        }

        return false;
    }

    public HashSet<string> GetValidFields(
        IEnumerable<string> fieldMask,
        object entityToSerialize)
    {
        var fieldSet = new HashSet<string>(fieldMask.Select(f => f.ToLower()));
        var globalWildcard = fieldSet.Contains("*") || fieldSet.Count == 0;

        if (!globalWildcard)
        {
            var wildcards =
                fieldSet
                    .Where(f => f.EndsWith(".*"))
                    .Select(f => f.Split('.')[0])
                    .ToList();

            fieldSet =
                wildcards
                    .Aggregate(fieldSet,
                        (current, wildcard) => ExpandNestedFields(wildcard, entityToSerialize, current));
        }

        var availablePaths = GetAllFieldPaths(entityToSerialize).ToHashSet();
        fieldSet.RemoveWhere(f => !availablePaths.Contains(f));

        return fieldSet;
    }

    private static HashSet<string> ExpandNestedFields(
        string rootProperty,
        object entityToSerialize,
        HashSet<string> existingFields)
    {
        var rootPropertyInfo = GetComplexPropertyInfo(rootProperty, entityToSerialize);

        return rootPropertyInfo == null
            ? existingFields
            : AddSubProperties(rootProperty, rootPropertyInfo, existingFields);
    }

    private static PropertyInfo? GetComplexPropertyInfo(
        string rootProperty,
        object entityToSerialize)
    {
        var camelCaseRootProperty = ConvertToCamelCase(rootProperty);
        var propertyInfo = entityToSerialize
            .GetType()
            .GetProperty(camelCaseRootProperty, BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo == null || !TypeDetector.IsComplexType(propertyInfo.PropertyType))
            return null;

        return propertyInfo;
    }

    private static HashSet<string> AddSubProperties(
        string rootProperty,
        PropertyInfo rootPropertyInfo,
        HashSet<string> existingFields)
    {
        var subProperties = rootPropertyInfo.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var newFields = new HashSet<string>(existingFields);

        foreach (var subProperty in subProperties)
        {
            var subPropertyPath = $"{rootProperty}.{subProperty.Name.ToLower()}";
            newFields.Add(subPropertyPath);
        }

        return newFields;
    }

    private static string ConvertToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsUpper(input[0]))
            return input;

        return char.ToUpper(input[0]) + input[1..];
    }

    [GeneratedRegex(@"\b\w*response\.\b", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveResponseRegex();

    [GeneratedRegex(@"\b\w*request\.\b", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveRequestRegex();

    [GeneratedRegex("contract", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveContractRegex();

    private static string RemoveRegex(
        string input,
        params Regex[] patterns)
    {
        foreach (var pattern in patterns)
        {
            input = pattern.Replace(input, "").Trim();
        }

        return input;
    }

    private static List<string?> GetAllFieldPaths(object entityToSerialize)
    {
        var availableFieldMasks = new List<string?>();
        GatherNestedPaths(entityToSerialize, availableFieldMasks, null);
        return availableFieldMasks;
    }

    // Todo: Break this down further
    private static void GatherNestedPaths(
        object resource,
        List<string?> fieldMask,
        string? prefix)
    {
        var type = resource.GetType();
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propertyValue = property.GetValue(resource);
            var propertyName = prefix != null ? $"{prefix}.{property.Name.ToLower()}" : property.Name.ToLower();

            if (propertyValue == null)
            {
            }
            else if (IsCollection(propertyName))
            {
                if (propertyValue is not IEnumerable collection) return;
                foreach (var item in collection)
                {
                    GatherNestedPaths(item, fieldMask, propertyName);
                    break; // Only infer once for collections
                }
            }
            else if (IsNestedObject(propertyValue))
            {
                fieldMask.Add(propertyName + ".*");
                GatherNestedPaths(propertyValue, fieldMask, propertyName);
            }
            else
            {
                fieldMask.Add(propertyName);
            }
        }
    }

    private static string BuildFullPath(MemberInfo member)
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

    private static bool IsCollection(object? propertyValue)
    {
        return propertyValue != null && propertyValue is not string && propertyValue is IEnumerable;
    }

    private static bool IsNestedObject(object? propertyValue)
    {
        return propertyValue != null && propertyValue.GetType().IsClass && propertyValue.GetType() != typeof(string);
    }
}