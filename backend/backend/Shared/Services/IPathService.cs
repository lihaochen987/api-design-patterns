using System.Reflection;

namespace backend.Shared.Services;

public interface IPathService
{
    string GetPropertyPath(MemberInfo member);
    string GeneratePropertyName(string? prefix, string propertyName);
}