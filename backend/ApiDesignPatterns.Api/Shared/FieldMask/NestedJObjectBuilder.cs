using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Shared.FieldMask;

public class NestedJObjectBuilder
{
    public JObject Build(
        PropertyInfo property,
        object instance,
        JsonSerializer serializer,
        HashSet<string> expandedFieldMask)
    {
        JObject jObject = new();
        object? nestedInstance = property.GetValue(instance);

        if (nestedInstance == null)
        {
            return jObject;
        }

        PropertyInfo[] nestedProperties =
            property.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo nestedProperty in nestedProperties)
        {
            string nestedPath = $"{property.Name.ToLowerInvariant()}.{nestedProperty.Name.ToLowerInvariant()}";
            if (!expandedFieldMask.Contains(nestedPath))
            {
                continue;
            }

            object? nestedValue = nestedProperty.GetValue(nestedInstance);
            if (nestedValue != null)
            {
                jObject.Add(nestedProperty.Name, JToken.FromObject(nestedValue, serializer));
            }
        }

        return jObject;
    }
}
