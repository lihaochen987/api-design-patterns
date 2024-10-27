namespace backend.Shared.Utility;

public static class TypeHelper
{
    public static bool IsComplexType(Type type)
    {
        return !type.IsPrimitive && type != typeof(string) && !type.IsEnum;
    }
}