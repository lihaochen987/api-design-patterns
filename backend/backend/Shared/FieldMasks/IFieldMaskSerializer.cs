using System.Reflection;

namespace backend.Shared.FieldMasks;

public interface IFieldMaskSerializer
{
    bool IsJsonPropertySerializable(MemberInfo member, IEnumerable<string> fieldMask, object entityToSerialize);
}