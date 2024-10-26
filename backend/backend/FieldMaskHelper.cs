using System.Collections;
using System.Reflection;

namespace backend;

public static class FieldMaskHelper
{
    public static List<string> InferFieldMask(object resource)
    {
        var availableFieldMasks = new List<string>();
        InferFieldMaskRecursive(resource, availableFieldMasks, null);
        return availableFieldMasks;
    }

    private static void InferFieldMaskRecursive(object resource, List<string> fieldMask, string prefix)
    {
        if (resource == null) return;

        var type = resource.GetType();
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propertyValue = property.GetValue(resource);
            var propertyName = prefix != null ? $"{prefix}.{property.Name.ToLower()}" : property.Name.ToLower();

            if (propertyValue != null && propertyValue is not string && propertyValue is IEnumerable nestedEnumerable)
            {
                // Handle collection of objects (e.g., List<Dimension> or arrays)
                foreach (var nestedItem in nestedEnumerable)
                {
                    InferFieldMaskRecursive(nestedItem, fieldMask, propertyName);
                    break; // Only infer once for collections
                }
            }
            else if (propertyValue != null && propertyValue.GetType().IsClass &&
                     propertyValue.GetType() != typeof(string))
            {
                // Recursively handle nested objects
                fieldMask.Add(propertyName + ".*");
                InferFieldMaskRecursive(propertyValue, fieldMask, propertyName);
            }
            else if (propertyValue != null) // Only add if property is not null
            {
                fieldMask.Add(propertyName);
            }
        }
    }
}