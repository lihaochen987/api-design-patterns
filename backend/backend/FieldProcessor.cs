using System.Reflection;

namespace backend;

public class FieldProcessor
{
    public static HashSet<string> PrepareFields(IEnumerable<string> fields, object targetObject)
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

        var availablePaths = FieldMaskHelper.InferFieldMask(targetObject).ToHashSet();
        fieldSet.RemoveWhere(f => !availablePaths.Contains(f));

        return fieldSet;
    }

    private static HashSet<string> AddSubPropertiesToFields(
        string rootProperty, object targetObject, HashSet<string> existingFields)
    {
        var camelCaseRootProperty = StringHelper.ToCamelCase(rootProperty);
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
}