using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Shared.FieldMask;

/// <summary>
/// JSON converter that filters object properties using field masks. Supports nested paths and wildcards.
/// </summary>
/// <example>
/// Field mask ["name", "pricing.basePrice"]:
/// {
///   "name": "Premium Dog Food",
///   "pricing": {
///     "basePrice": 29.99
///   }
/// }
/// </example>
public class FieldMaskConverter(
    IList<string> fieldMask,
    HashSet<string> allFieldPaths)
    : JsonConverter
{
    private readonly HashSet<string> _expandedFieldMask = Expand(fieldMask, allFieldPaths);

    /// <summary>
    /// Checks if type can be converted.
    /// </summary>
    public override bool CanConvert(Type objectType) => objectType.IsClass && objectType != typeof(string);

    /// <summary>
    /// Writes JSON using the field mask.
    /// </summary>
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
                AddPropertyToJObject(jObject, property, value, serializer, _expandedFieldMask);
            }
        }

        jObject.WriteTo(writer);
    }

    /// <summary>
    /// Reading JSON is not supported.
    /// </summary>
    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object? existingValue,
        JsonSerializer serializer) =>
        throw new NotImplementedException();

    /// <summary>
    /// Adds property to JSON object if included in field mask.
    /// </summary>
    private static void AddPropertyToJObject(
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
            JObject nestedObject = Build(property, value, serializer, expandedFieldMask);
            if (nestedObject.HasValues)
            {
                jObject.Add(property.Name, nestedObject);
            }
        }
    }

    /// <summary>
    /// Builds JSON object for nested properties.
    /// </summary>
    private static JObject Build(
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


    /// <summary>
    /// Expands field masks with wildcards into explicit paths.
    /// </summary>
    /// <example>
    /// ["user.*"] expands to ["user.id", "user.name", "user.email"]
    /// </example>
    private static HashSet<string> Expand(IList<string> fieldMask, HashSet<string> allFieldPaths)
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
}
