using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Shared;

public class FieldMaskConverter(
    IList<string> fieldMask,
    HashSet<string> allFieldPaths)
    : JsonConverter
{
    private readonly HashSet<string> _expandedFieldMask = ExpandFieldMask(fieldMask, allFieldPaths);

    private static HashSet<string> ExpandFieldMask(
        IList<string> fieldMask,
        HashSet<string> allFieldPaths)
    {
        HashSet<string> expandedFields = new(StringComparer.OrdinalIgnoreCase);

        if (fieldMask.Contains("*") || !fieldMask.Any())
        {
            return allFieldPaths;
        }

        foreach (string field in fieldMask.Select(f => f.ToLowerInvariant()))
        {
            if (field.EndsWith(".*"))
            {
                // The prefix would remove the .* at the end of the property
                string prefix = field[..^2];
                expandedFields.UnionWith(allFieldPaths.Where(p => p.StartsWith(prefix + ".")));
            }
            else if (allFieldPaths.Contains(field))
            {
                expandedFields.Add(field);
            }
        }

        return expandedFields.Count == 0 ? allFieldPaths : expandedFields;
    }

    public override bool CanConvert(Type objectType) => objectType.IsClass && objectType != typeof(string);

    public override void WriteJson(
        JsonWriter writer,
        object? value,
        JsonSerializer serializer)
    {
        JObject jObject = new();
        if (value != null)
        {
            PropertyInfo[] properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                AddPropertyToJObject(jObject, property, value, serializer);
            }
        }

        jObject.WriteTo(writer);
    }

    private void AddPropertyToJObject(
        JObject jObject,
        PropertyInfo property,
        object value,
        JsonSerializer serializer)
    {
        string propertyPath = property.Name.ToLowerInvariant();

        if (_expandedFieldMask.Contains(propertyPath))
        {
            object? propValue = property.GetValue(value);
            if (propValue != null)
            {
                jObject.Add(property.Name, JToken.FromObject(propValue, serializer));
            }
        }
        else if (IsComplexType(property.PropertyType))
        {
            JObject nestedObject = BuildNestedJObject(property, value, serializer, _expandedFieldMask);
            if (nestedObject.HasValues)
            {
                jObject.Add(property.Name, nestedObject);
            }
        }
    }

    private static JObject BuildNestedJObject(
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

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object? existingValue,
        JsonSerializer serializer) =>
        throw new NotImplementedException();

    private static bool IsComplexType(Type type) => type.IsClass && type != typeof(string);
}
