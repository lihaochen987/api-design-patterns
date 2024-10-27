using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using backend.Shared.Utility;

namespace backend.Shared;

public partial class FieldMaskSerializer
{
    // Todo: Break this down further
    public bool IsJsonPropertySerializable(
        MemberInfo member,
        IEnumerable<string> fieldMask,
        object entityToSerialize)
    {
        var fields = ValidFields(fieldMask, entityToSerialize);

        if (fields.Contains("*") || fields.Count == 0)
        {
            return true;
        }

        // Todo: this is grotty
        var propertyName = BuildFullPath(member);
        var propertyNameWithoutPrefix = RemoveContractPattern(
            RemoveResponsePattern(propertyName)).Trim().ToLowerInvariant();

        if (fields.Contains(propertyNameWithoutPrefix))
            return true;

        if (member is PropertyInfo propertyInfo && TypeHelper.IsComplexType(propertyInfo.PropertyType))
        {
            return propertyInfo.PropertyType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(subProperty => $"{propertyNameWithoutPrefix}.{subProperty.Name.ToLower()}")
                .Any(fields.Contains);
        }

        return false;
    }

    private HashSet<string> ValidFields(IEnumerable<string> fields, object targetObject)
    {
        var fieldSet = new HashSet<string>(fields.Select(f => f.ToLower()));
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
                        (current, wildcard) => ExpandWithNestedFields(wildcard, targetObject, current));
        }

        var availablePaths = GetAvailablePaths(targetObject).ToHashSet();
        fieldSet.RemoveWhere(f => !availablePaths.Contains(f));

        return fieldSet;
    }


    // Todo: Break this down further
    private static HashSet<string> ExpandWithNestedFields(
        string rootProperty,
        object targetObject,
        HashSet<string> existingFields)
    {
        var camelCaseRootProperty = ConvertToCamelCase(rootProperty);
        var rootPropertyInfo = targetObject
            .GetType()
            .GetProperty(camelCaseRootProperty, BindingFlags.Public | BindingFlags.Instance);

        if (rootPropertyInfo == null || !TypeHelper.IsComplexType(rootPropertyInfo.PropertyType))
            return existingFields;

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

    private static string RemoveResponsePattern(string input)
    {
        return RemoveResponseRegex().Replace(input, "").Trim();
    }

    [GeneratedRegex("contract", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveContractRegex();

    private static string RemoveContractPattern(string input)
    {
        return RemoveContractRegex().Replace(input, "").Trim();
    }

    private List<string?> GetAvailablePaths(object resource)
    {
        var availableFieldMasks = new List<string?>();
        GatherNestedPaths(resource, availableFieldMasks, null);
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
            else if (IsCollection(propertyValue))
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