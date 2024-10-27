using System.Reflection;

namespace backend.Shared.Services;

public interface ISerializationService
{
    bool ShouldSerializeJsonProperty(MemberInfo member, IEnumerable<string> fields, object targetObject);
}