using System.Reflection;
using backend.Shared.Utility;

namespace backend.Shared.FieldMasks;

public class FieldMaskSerializer(
    IFieldMaskSelector fieldMaskSelector,
    IFieldMaskPathBuilder fieldMaskPathBuilder,
    IFieldMaskPatternCleaner fieldMaskPatternCleaner)
    : IFieldMaskSerializer
{
    public bool IsJsonPropertySerializable(MemberInfo member, IEnumerable<string> fieldMask, object entityToSerialize)
    {
        var parsedFields = fieldMaskSelector.ValidFields(fieldMask, entityToSerialize);

        if (parsedFields.Contains("*") || parsedFields.Count == 0)
        {
            return true;
        }

        var propertyName = fieldMaskPathBuilder.BuildFullPath(member);
        var propertyNameWithoutPrefix = fieldMaskPatternCleaner.RemoveContractPattern(
            fieldMaskPatternCleaner.RemoveResponsePattern(propertyName)).Trim().ToLowerInvariant();

        if (parsedFields.Contains(propertyNameWithoutPrefix))
            return true;

        if (member is PropertyInfo propertyInfo && TypeHelper.IsComplexType(propertyInfo.PropertyType))
        {
            return propertyInfo.PropertyType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(subProperty => $"{propertyNameWithoutPrefix}.{subProperty.Name.ToLower()}")
                .Any(parsedFields.Contains);
        }

        return false;
    }
}