namespace backend.Shared.Utility;

public interface IReflectionUtility
{
    bool IsCollection(object? propertyValue);
    bool IsNestedObject(object? propertyValue);
}