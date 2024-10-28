namespace backend.Product;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class ProductFieldMaskConverter(IEnumerable<string> fieldMask) : JsonConverter
{
    private readonly HashSet<string> _expandedFieldMask = ExpandFieldMask(fieldMask);

    private static HashSet<string> ExpandFieldMask(IEnumerable<string> fieldMask)
    {
        // Define all possible field paths for Product, including nested fields
        var allFieldPaths = new HashSet<string>
        {
            "*",
            "id",
            "name",
            "price",
            "category",
            "dimensions.*",
            "dimensions.width",
            "dimensions.height",
            "dimensions.length"
        };

        var expandedFields = new HashSet<string>();

        if (fieldMask.Contains("*") || !fieldMask.Any())
        {
            return allFieldPaths;
        }

        foreach (var field in fieldMask.Select(f => f.ToLowerInvariant()))
        {
            if (field.EndsWith(".*"))
            {
                // Expand wildcard entries, e.g., "dimensions.*" to all nested properties
                var prefix = field.Substring(0, field.Length - 2);
                expandedFields.UnionWith(allFieldPaths.Where(p => p.StartsWith(prefix + ".")));
            }
            else if (allFieldPaths.Contains(field))
            {
                expandedFields.Add(field);
            }
        }

        if (!expandedFields.Any())
        {
            return allFieldPaths;
        }

        return expandedFields;
    }

    public override bool CanConvert(Type objectType)
    {
        // Allow conversion for complex types (Product in this case)
        return objectType.IsClass && objectType != typeof(string);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var jObject = new JObject();
        var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var propertyPath = property.Name.ToLowerInvariant();

            // Check if the property itself is serializable based on the field mask
            if (_expandedFieldMask.Contains(propertyPath))
            {
                var propValue = property.GetValue(value);
                if (propValue != null)
                {
                    jObject.Add(property.Name, JToken.FromObject(propValue, serializer));
                }
            }
            // If the property is complex, handle nested serialization
            else if (IsComplexType(property.PropertyType))
            {
                var nestedObject = SerializeComplexProperty(property, value, serializer);
                if (nestedObject.HasValues) // Avoid empty objects
                {
                    jObject.Add(property.Name, nestedObject);
                }
            }
        }

        jObject.WriteTo(writer);
    }

    private JObject SerializeComplexProperty(PropertyInfo property, object instance, JsonSerializer serializer)
    {
        var jObject = new JObject();
        var nestedInstance = property.GetValue(instance);

        if (nestedInstance == null)
            return jObject;

        var nestedProperties = property.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var nestedProperty in nestedProperties)
        {
            var nestedPath = $"{property.Name.ToLowerInvariant()}.{nestedProperty.Name.ToLowerInvariant()}";
            if (_expandedFieldMask.Contains(nestedPath))
            {
                var nestedValue = nestedProperty.GetValue(nestedInstance);
                if (nestedValue != null)
                {
                    jObject.Add(nestedProperty.Name, JToken.FromObject(nestedValue, serializer));
                }
            }
        }

        return jObject;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException("This converter is for write-only serialization.");
    }

    private bool IsComplexType(Type type)
    {
        // Determine if the type is complex (e.g., class types excluding strings)
        return type.IsClass && type != typeof(string);
    }
}