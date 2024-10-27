using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace backend;

public partial class FieldMask
{
    public bool ShouldSerialize(MemberInfo member, IEnumerable<string> fields, object targetObject)
    {
        var parsedFields = PrepareFields(fields, targetObject);

        if (parsedFields.Contains("*") || parsedFields.Count == 0)
        {
            return true;
        }

        var propertyName = GetPropertyPath(member);
        var propertyNameWithoutPrefix = RemoveContractFromString(
            RemoveResponseFromString(propertyName)).Trim().ToLowerInvariant();

        if (parsedFields.Contains(propertyNameWithoutPrefix))
            return true;

        if (member is PropertyInfo propertyInfo && TypeHelper.IsComplexType(propertyInfo.PropertyType))
        {
            return propertyInfo.PropertyType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(subProperty => $"{propertyNameWithoutPrefix}.{subProperty.Name.ToLower()}")
                .Any(parsedFields.Contains);
        }

        return false;
    }

    private static string GetPropertyPath(MemberInfo member)
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

    private static List<string?> GetAllFields(object resource)
    {
        var availableFieldMasks = new List<string?>();
        InferFieldMaskRecursive(resource, availableFieldMasks, null);
        return availableFieldMasks;
    }

    // Todo: try and merge this with GetAllFields
    private static void InferFieldMaskRecursive(
        object resource,
        List<string?> fieldMask,
        string? prefix)
    {
        var type = resource.GetType();
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propertyValue = property.GetValue(resource);
            var propertyName = GeneratePropertyName(prefix, property.Name);

            if (propertyValue == null)
            {
            }
            else if (IsCollection(propertyValue))
            {
                if (propertyValue is not IEnumerable collection) return;
                foreach (var item in collection)
                {
                    InferFieldMaskRecursive(item, fieldMask, propertyName);
                    break; // Only infer once for collections
                }
            }
            else if (IsNestedObject(propertyValue))
            {
                fieldMask.Add(propertyName + ".*");
                InferFieldMaskRecursive(propertyValue, fieldMask, propertyName);
            }
            else
            {
                fieldMask.Add(propertyName);
            }
        }
    }

    private static string GeneratePropertyName(
        string? prefix,
        string propertyName)
    {
        return prefix != null ? $"{prefix}.{propertyName.ToLower()}" : propertyName.ToLower();
    }

    private static bool IsCollection(object? propertyValue)
    {
        return propertyValue != null && propertyValue is not string && propertyValue is IEnumerable;
    }

    private static bool IsNestedObject(object? propertyValue)
    {
        return propertyValue != null && propertyValue.GetType().IsClass && propertyValue.GetType() != typeof(string);
    }

    private static HashSet<string> PrepareFields(IEnumerable<string> fields, object targetObject)
    {
        var fieldSet = new HashSet<string>(fields.Select(f => f.ToLower()));
        var globalWildcard = fieldSet.Contains("*") || fieldSet.Count == 0;

        if (!globalWildcard)
        {
            var wildcards = fieldSet.Where(f => f.EndsWith(".*"))
                .Select(f => f.Split('.')[0])
                .ToList();

            fieldSet = wildcards.Aggregate(fieldSet,
                (current, wildcard) => AddSubPropertiesToFields(wildcard, targetObject, current));
        }

        var availablePaths = GetAllFields(targetObject).ToHashSet();
        fieldSet.RemoveWhere(f => !availablePaths.Contains(f));

        return fieldSet;
    }

    private static HashSet<string> AddSubPropertiesToFields(
        string rootProperty, object targetObject, HashSet<string> existingFields)
    {
        var camelCaseRootProperty = ToCamelCase(rootProperty);
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

    private static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsUpper(input[0]))
            return input;

        return char.ToUpper(input[0]) + input[1..];
    }

    private static string RemoveResponseFromString(string input)
    {
        return RemoveResponseRegex().Replace(input, "").Trim();
    }

    private static string RemoveContractFromString(string input)
    {
        return RemoveContractRegex().Replace(input, "").Trim();
    }

    [GeneratedRegex(@"\b\w*response\.\b", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveResponseRegex();

    [GeneratedRegex("contract", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex RemoveContractRegex();
}