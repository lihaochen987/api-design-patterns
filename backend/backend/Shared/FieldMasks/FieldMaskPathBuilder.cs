using System.Reflection;

namespace backend.Shared.FieldMasks;

public class FieldMaskPathBuilder : IFieldMaskPathBuilder
{
    public string BuildFullPath(MemberInfo member)
    {
        var path = member.Name;
        var declaringType = member.DeclaringType;

        while (declaringType != null)
        {
            path = declaringType.Name.ToLower() + "." + path;
            declaringType = declaringType.DeclaringType;
        }

        return path;
    }

    public string GeneratePropertyName(
        string? prefix,
        string propertyName)
    {
        return prefix != null ? $"{prefix}.{propertyName.ToLower()}" : propertyName.ToLower();
    }
}