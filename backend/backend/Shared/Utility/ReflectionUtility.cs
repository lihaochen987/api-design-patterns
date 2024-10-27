using System.Collections;

namespace backend.Shared.Utility;

public class ReflectionUtility : IReflectionUtility
{
    public bool IsCollection(object? propertyValue)
    {
        return propertyValue != null && propertyValue is not string && propertyValue is IEnumerable;
    }

    public bool IsNestedObject(object? propertyValue)
    {
        return propertyValue != null && propertyValue.GetType().IsClass && propertyValue.GetType() != typeof(string);
    }
}