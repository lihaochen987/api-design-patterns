using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Shared.FieldMask;

public class FieldMaskConverter(
    IList<string> fieldMask,
    HashSet<string> allFieldPaths,
    FieldMaskExpander expander,
    PropertyHandler propertyHandler)
    : JsonConverter
{
    private readonly HashSet<string> _expandedFieldMask = expander.Expand(fieldMask, allFieldPaths);

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
                propertyHandler.AddPropertyToJObject(jObject, property, value, serializer, _expandedFieldMask);
            }
        }

        jObject.WriteTo(writer);
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object? existingValue,
        JsonSerializer serializer) =>
        throw new NotImplementedException();
}
