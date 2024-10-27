using System.Collections;
using System.Reflection;
using backend.Shared.Utility;

namespace backend.Shared.FieldMasks;

public class FieldMaskSelector(
    IReflectionUtility reflectionUtility,
    IFieldMaskPathBuilder fieldMaskPathBuilder)
    : IFieldMaskSelector
{
    public HashSet<string> ValidFields(IEnumerable<string> fields, object targetObject)
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

        var availablePaths = GetAvailableFieldPaths(targetObject).ToHashSet();
        fieldSet.RemoveWhere(f => !availablePaths.Contains(f));

        return fieldSet;
    }

    public List<string?> GetAvailableFieldPaths(object resource)
    {
        var availableFieldMasks = new List<string?>();
        GatherNestedFieldMaskPaths(resource, availableFieldMasks, null);
        return availableFieldMasks;
    }

    private HashSet<string> ExpandWithNestedFields(
        string rootProperty, object targetObject, HashSet<string> existingFields)
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

    private string ConvertToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsUpper(input[0]))
            return input;

        return char.ToUpper(input[0]) + input[1..];
    }

    // Todo: try and merge this with GetAllFields
    private void GatherNestedFieldMaskPaths(
        object resource,
        List<string?> fieldMask,
        string? prefix)
    {
        var type = resource.GetType();
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propertyValue = property.GetValue(resource);
            var propertyName = fieldMaskPathBuilder.GeneratePropertyName(prefix, property.Name);

            if (propertyValue == null)
            {
            }
            else if (reflectionUtility.IsCollection(propertyValue))
            {
                if (propertyValue is not IEnumerable collection) return;
                foreach (var item in collection)
                {
                    GatherNestedFieldMaskPaths(item, fieldMask, propertyName);
                    break; // Only infer once for collections
                }
            }
            else if (reflectionUtility.IsNestedObject(propertyValue))
            {
                fieldMask.Add(propertyName + ".*");
                GatherNestedFieldMaskPaths(propertyValue, fieldMask, propertyName);
            }
            else
            {
                fieldMask.Add(propertyName);
            }
        }
    }
}