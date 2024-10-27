using System.Reflection;
using backend.Shared.Utility;

namespace backend.Shared.Services;

public class SerializationService(
    IFieldPreparationService fieldPreparationService,
    IPathService pathService,
    IRegexService regexService)
    : ISerializationService
{
    public bool ShouldSerializeJsonProperty(MemberInfo member, IEnumerable<string> fields, object targetObject)
    {
        var parsedFields = fieldPreparationService.PrepareFields(fields, targetObject);

        if (parsedFields.Contains("*") || parsedFields.Count == 0)
        {
            return true;
        }

        var propertyName = pathService.GetPropertyPath(member);
        var propertyNameWithoutPrefix = regexService.RemoveContractFromString(
            regexService.RemoveHcLc(propertyName)).Trim().ToLowerInvariant();

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