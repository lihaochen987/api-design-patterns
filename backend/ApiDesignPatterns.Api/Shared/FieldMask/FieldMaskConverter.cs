using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Shared.FieldMask;

/// <summary>
/// A custom JSON converter that selectively serializes object properties based on a field mask.
/// </summary>
/// <remarks>
/// This converter supports nested object serialization and wildcard patterns in the field mask.
/// Field paths are case-insensitive.
/// </remarks>
/// <example>
/// Given an object:
/// <code>
/// {
///   "name": "Premium Dog Food",
///   "pricing": {
///     "basePrice": 29.99,
///     "taxRate": 8.5,
///     "discountPercentage": 10.0
///   },
///   "category": {
///     "id": 1,
///     "name": "Pet Food"
///   }
/// }
/// </code>
///
/// With field mask ["name", "pricing.basePrice"]:
/// <code>
/// {
///   "name": "Premium Dog Food",
///   "pricing": {
///     "basePrice": 29.99
///   }
/// }
/// </code>
///
/// With field mask ["category.*"]:
/// <code>
/// {
///   "category": {
///     "id": 1,
///     "name": "Pet Food"
///   }
/// }
/// </code>
/// </example>
public class FieldMaskConverter(
    IList<string> fieldMask,
    HashSet<string> allFieldPaths)
    : JsonConverter
{
    private readonly HashSet<string> _expandedFieldMask = Expand(fieldMask, allFieldPaths);

    /// <summary>
    /// Determines if this converter can convert the specified type.
    /// </summary>
    /// <param name="objectType">Type to check for conversion compatibility.</param>
    /// <returns>True if the type is a class and not a string, false otherwise.</returns>
    public override bool CanConvert(Type objectType) => objectType.IsClass && objectType != typeof(string);

    /// <summary>
    /// Writes the JSON representation of the object using the specified field mask.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="serializer">The calling serializer.</param>
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
    /// Reading JSON is not supported by this converter.
    /// </summary>
    /// <exception cref="NotImplementedException">Always thrown as reading is not supported.</exception>
    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object? existingValue,
        JsonSerializer serializer) =>
        throw new NotImplementedException();

    /// <summary>
    /// Adds a property to the JSON object based on the field mask.
    /// </summary>
    /// <param name="jObject">The JSON object to add the property to.</param>
    /// <param name="property">The property information.</param>
    /// <param name="value">The object instance containing the property.</param>
    /// <param name="serializer">The JSON serializer.</param>
    /// <param name="expandedFieldMask">The expanded field mask paths.</param>
    /// <remarks>
    /// If the property is in the field mask, it's added directly.
    /// If it's a complex type, it's processed recursively.
    /// </remarks>
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
    /// Builds a JSON object for a nested property based on the field mask.
    /// </summary>
    /// <param name="property">The property information.</param>
    /// <param name="instance">The parent object instance.</param>
    /// <param name="serializer">The JSON serializer.</param>
    /// <param name="expandedFieldMask">The expanded field mask paths.</param>
    /// <returns>A JObject containing the nested property's serialized values.</returns>
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
    /// Expands the field mask to include all matching paths.
    /// </summary>
    /// <param name="fieldMask">The original field mask.</param>
    /// <param name="allFieldPaths">All possible field paths in the object model.</param>
    /// <returns>A HashSet of expanded field paths.</returns>
    /// <example>
    /// Input:
    /// <code>
    /// fieldMask = ["user.*", "order.id"]
    /// allFieldPaths = [
    ///   "user.id",
    ///   "user.name",
    ///   "user.email",
    ///   "order.id",
    ///   "order.date"
    /// ]
    /// </code>
    ///
    /// Output:
    /// <code>
    /// [
    ///   "user.id",
    ///   "user.name",
    ///   "user.email",
    ///   "order.id"
    /// ]
    /// </code>
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
