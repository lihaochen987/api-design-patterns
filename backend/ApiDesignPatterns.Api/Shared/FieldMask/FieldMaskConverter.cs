using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Shared.FieldMask;

/// <summary>
/// JSON converter that filters object properties using field masks. Supports nested paths, wildcards, and lists.
/// </summary>
/// <example>
/// Field mask ["name", "pricing.basePrice", "variants[].color"]:
/// {
///   "name": "Premium Dog Food",
///   "pricing": {
///     "basePrice": 29.99
///   },
///   "variants": [
///     { "color": "brown" },
///     { "color": "black" }
///   ]
/// }
/// </example>
public class FieldMaskConverter(
    HashSet<string> expandedFieldMasks,
    JsonSerializer jsonSerializer)
    : JsonConverter
{
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
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        // Handle collections (lists, arrays, etc.)
        if (IsCollection(value.GetType()) && value is IEnumerable collection)
        {
            writer.WriteStartArray();
            foreach (object? item in collection)
            {
                WriteCollectionTypeObject(item, writer);
            }

            writer.WriteEndArray();
        }
        else
        {
            // Handle regular objects
            WriteComplexObject(writer, value, "");
        }
    }

    private void WriteCollectionTypeObject(object? item, JsonWriter writer)
    {
        if (item == null)
        {
            writer.WriteNull();
        }
        else if (item.GetType().IsClass && item.GetType() != typeof(string))
        {
            WriteComplexObject(writer, item, "");
        }
        else
        {
            JToken.FromObject(item, jsonSerializer).WriteTo(writer);
        }
    }

    /// <summary>
    /// Writes a complex object as JSON with field masking applied.
    /// </summary>
    private void WriteComplexObject(
        JsonWriter writer,
        object value,
        string parentPath)
    {
        JObject jObject = new();
        PropertyInfo[] properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            AddPropertyToJObject(jObject, property, value, parentPath);
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
    /// Checks if a type is a collection type.
    /// </summary>
    private static bool IsCollection(Type type)
    {
        // Check if it's a collection but not a string
        return typeof(IEnumerable).IsAssignableFrom(type)
               && type != typeof(string);
    }

    /// <summary>
    /// Adds property to JSON object if included in field mask.
    /// </summary>
    private void AddPropertyToJObject(
        JObject jObject,
        PropertyInfo property,
        object value,
        string parentPath)
    {
        string propertyName = property.Name;
        string currentPath = string.IsNullOrEmpty(parentPath)
            ? propertyName.ToLowerInvariant()
            : $"{parentPath}.{propertyName.ToLowerInvariant()}";

        object? propValue = property.GetValue(value);
        if (propValue == null)
        {
            return;
        }

        // Direct property match
        if (expandedFieldMasks.Contains(currentPath))
        {
            jObject.Add(propertyName, JToken.FromObject(propValue, jsonSerializer));
            return;
        }

        // Handle collections
        if (IsCollection(property.PropertyType) && propValue is IEnumerable collection)
        {
            // Check if collection has any field masks like "items[].property"
            string collectionWildcardPath = $"{currentPath}.";
            bool hasCollectionMasks = expandedFieldMasks.Any(mask => mask.StartsWith(collectionWildcardPath));

            if (!hasCollectionMasks)
            {
                return;
            }

            JArray jArray = [];

            foreach (object? item in collection)
            {
                if (item == null)
                {
                    jArray.Add(JValue.CreateNull());
                }
                else if (item.GetType().IsClass && item.GetType() != typeof(string))
                {
                    // Process complex items in the collection
                    JObject itemObject = ProcessCollectionItem(item, currentPath);
                    if (itemObject.HasValues)
                    {
                        jArray.Add(itemObject);
                    }
                }
                else
                {
                    // For primitive items
                    jArray.Add(JToken.FromObject(item, jsonSerializer));
                }
            }

            if (jArray.Count > 0)
            {
                jObject.Add(propertyName, jArray);
            }
        }
        // Handle nested objects
        else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
        {
            JObject nestedObject = BuildNestedObject(propValue, currentPath);
            if (nestedObject.HasValues)
            {
                jObject.Add(propertyName, nestedObject);
            }
        }
    }

    /// <summary>
    /// Process a single item in a collection.
    /// </summary>
    private JObject ProcessCollectionItem(
        object item,
        string collectionPath)
    {
        JObject itemObject = new();
        string collectionWildcardPath = $"{collectionPath}.";

        PropertyInfo[] itemProperties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var itemProperty in itemProperties)
        {
            string itemPropertyPath = $"{collectionWildcardPath}{itemProperty.Name.ToLowerInvariant()}";

            if (expandedFieldMasks.Contains(itemPropertyPath))
            {
                object? itemPropValue = itemProperty.GetValue(item);
                if (itemPropValue != null)
                {
                    itemObject.Add(itemProperty.Name, JToken.FromObject(itemPropValue, jsonSerializer));
                }
            }
            else if (itemProperty.PropertyType.IsClass && itemProperty.PropertyType != typeof(string))
            {
                object? nestedItemValue = itemProperty.GetValue(item);
                if (nestedItemValue == null)
                {
                    continue;
                }

                string nestedItemPath = $"{collectionWildcardPath}{itemProperty.Name.ToLowerInvariant()}";
                JObject nestedObject = BuildNestedObject(nestedItemValue, nestedItemPath);
                if (nestedObject.HasValues)
                {
                    itemObject.Add(itemProperty.Name, nestedObject);
                }
            }
        }

        return itemObject;
    }

    /// <summary>
    /// Builds JSON object for nested properties.
    /// </summary>
    private JObject BuildNestedObject(
        object instance,
        string parentPath)
    {
        JObject jObject = new();

        PropertyInfo[] nestedProperties = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo nestedProperty in nestedProperties)
        {
            string nestedPath = $"{parentPath}.{nestedProperty.Name.ToLowerInvariant()}";

            // Direct property match
            if (expandedFieldMasks.Contains(nestedPath))
            {
                object? nestedValue = nestedProperty.GetValue(instance);
                if (nestedValue != null)
                {
                    jObject.Add(nestedProperty.Name, JToken.FromObject(nestedValue, jsonSerializer));
                }
            }
            // Check if it's a collection
            else if (IsCollection(nestedProperty.PropertyType))
            {
                object? nestedValue = nestedProperty.GetValue(instance);
                if (nestedValue is not IEnumerable collection)
                {
                    continue;
                }

                string collectionWildcardPath = $"{nestedPath}[].";
                bool hasCollectionMasks = expandedFieldMasks.Any(mask => mask.StartsWith(collectionWildcardPath));

                if (!hasCollectionMasks)
                {
                    continue;
                }

                JArray jArray = [];

                foreach (object? item in collection)
                {
                    if (item == null)
                    {
                        jArray.Add(JValue.CreateNull());
                    }
                    else if (item.GetType().IsClass && item.GetType() != typeof(string))
                    {
                        JObject itemObject = ProcessCollectionItem(item, nestedPath);
                        if (itemObject.HasValues)
                        {
                            jArray.Add(itemObject);
                        }
                    }
                    else
                    {
                        jArray.Add(JToken.FromObject(item, jsonSerializer));
                    }
                }

                if (jArray.Count > 0)
                {
                    jObject.Add(nestedProperty.Name, jArray);
                }
            }
            // Check for nested objects
            else if (nestedProperty.PropertyType.IsClass && nestedProperty.PropertyType != typeof(string))
            {
                object? deepNestedInstance = nestedProperty.GetValue(instance);
                if (deepNestedInstance == null)
                {
                    continue;
                }

                JObject deepNestedObject = BuildNestedObject(deepNestedInstance, nestedPath);
                if (deepNestedObject.HasValues)
                {
                    jObject.Add(nestedProperty.Name, deepNestedObject);
                }
            }
        }

        return jObject;
    }
}
