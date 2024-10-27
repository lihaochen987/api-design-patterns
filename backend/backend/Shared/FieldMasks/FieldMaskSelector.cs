using System.Reflection;
using backend.Shared.Utility;

namespace backend.Shared.FieldMasks;

public class FieldMaskSelector(
    IFieldMaskPathResolver fieldMaskPathResolver)
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

        var availablePaths = fieldMaskPathResolver.GetAvailablePaths(targetObject).ToHashSet();
        fieldSet.RemoveWhere(f => !availablePaths.Contains(f));

        return fieldSet;
    }

    
    // Todo: Break this down further
    private HashSet<string> ExpandWithNestedFields(
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

    private string ConvertToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsUpper(input[0]))
            return input;

        return char.ToUpper(input[0]) + input[1..];
    }
}