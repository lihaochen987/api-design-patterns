namespace backend.Shared.Utilities;

public static class TypeDetector
{
    public static bool IsComplexType(Type type)
    {
        return !type.IsPrimitive && type != typeof(string) && !type.IsEnum;
    }
}