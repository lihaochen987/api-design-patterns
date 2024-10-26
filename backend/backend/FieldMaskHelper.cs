using System.Collections;
using System.Reflection;

namespace backend;

public static class FieldMaskHelper
{
    public static List<string?> InferFieldMask(object resource)
    {
        var availableFieldMasks = new List<string?>();
        InferFieldMaskRecursive(resource, availableFieldMasks, null);
        return availableFieldMasks;
    }

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
                HandleCollectionProperty(propertyValue, fieldMask, propertyName);
            }
            else if (IsNestedObject(propertyValue))
            {
                HandleNestedObject(propertyValue, fieldMask, propertyName);
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

    private static void HandleCollectionProperty(
        object propertyValue,
        List<string?> fieldMask,
        string? propertyName)
    {
        if (propertyValue is not IEnumerable collection) return;
        foreach (var item in collection)
        {
            InferFieldMaskRecursive(item, fieldMask, propertyName);
            break; // Only infer once for collections
        }
    }

    private static void HandleNestedObject(
        object propertyValue,
        List<string?> fieldMask,
        string? propertyName)
    {
        fieldMask.Add(propertyName + ".*");
        InferFieldMaskRecursive(propertyValue, fieldMask, propertyName);
    }
}