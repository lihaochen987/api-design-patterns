using System.Reflection;

namespace backend.Shared.FieldMasks;

public interface IFieldMaskPathBuilder
{
    string BuildFullPath(MemberInfo member);
}