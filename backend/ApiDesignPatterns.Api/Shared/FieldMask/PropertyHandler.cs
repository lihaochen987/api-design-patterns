using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Shared.FieldMask;

public class PropertyHandler(NestedJObjectBuilder nestedJObjectBuilder)
{
    public void AddPropertyToJObject(
        JObject jObject,
        PropertyInfo property,
        object value,
        JsonSerializer serializer,
        HashSet<string> expandedFieldMask)
    {
        string propertyPath = property.Name.ToLowerInvariant();

        if (expandedFieldMask.Contains(propertyPath))
        {
            object? propValue = property.GetValue(value);
            if (propValue != null)
            {
                jObject.Add(property.Name, JToken.FromObject(propValue, serializer));
            }
        }
        else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
        {
            JObject nestedObject = nestedJObjectBuilder.Build(property, value, serializer, expandedFieldMask);
            if (nestedObject.HasValues)
            {
                jObject.Add(property.Name, nestedObject);
            }
        }
    }
}
