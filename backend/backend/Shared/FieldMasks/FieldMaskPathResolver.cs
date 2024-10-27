using System.Collections;
using System.Reflection;
using backend.Shared.Utility;

namespace backend.Shared.FieldMasks;

public class FieldMaskPathResolver(
    IReflectionUtility reflectionUtility)
    : IFieldMaskPathResolver
{
    public List<string?> GetAvailablePaths(object resource)
    {
        var availableFieldMasks = new List<string?>();
        GatherNestedPaths(resource, availableFieldMasks, null);
        return availableFieldMasks;
    }

    // Todo: Break this down further
    private void GatherNestedPaths(
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
            else if (reflectionUtility.IsCollection(propertyValue))
            {
                if (propertyValue is not IEnumerable collection) return;
                foreach (var item in collection)
                {
                    GatherNestedPaths(item, fieldMask, propertyName);
                    break; // Only infer once for collections
                }
            }
            else if (reflectionUtility.IsNestedObject(propertyValue))
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
}