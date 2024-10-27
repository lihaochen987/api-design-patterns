using System.Reflection;
using backend.Shared.Utility;

namespace backend.Shared.FieldMasks;

public class FieldMaskSerializer(
    IFieldMaskSelector fieldMaskSelector,
    IFieldMaskPathBuilder fieldMaskPathBuilder,
    IFieldMaskPatternCleaner fieldMaskPatternCleaner)
    : IFieldMaskSerializer
{
    // Todo: Break this down further
    public bool IsJsonPropertySerializable(
        MemberInfo member, 
        IEnumerable<string> fieldMask, 
        object entityToSerialize)
    {
        var fields = fieldMaskSelector.ValidFields(fieldMask, entityToSerialize);

        if (fields.Contains("*") || fields.Count == 0)
        {
            return true;
        }

        // Todo: this is grotty
        var propertyName = fieldMaskPathBuilder.BuildFullPath(member);
        var propertyNameWithoutPrefix = fieldMaskPatternCleaner.RemoveContractPattern(
            fieldMaskPatternCleaner.RemoveResponsePattern(propertyName)).Trim().ToLowerInvariant();

        if (fields.Contains(propertyNameWithoutPrefix))
            return true;

        if (member is PropertyInfo propertyInfo && TypeHelper.IsComplexType(propertyInfo.PropertyType))
        {
            return propertyInfo.PropertyType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(subProperty => $"{propertyNameWithoutPrefix}.{subProperty.Name.ToLower()}")
                .Any(fields.Contains);
        }

        return false;
    }
}