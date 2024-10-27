namespace backend.Shared.FieldMasks;

public interface IFieldMaskPathResolver
{
    public List<string?> GetAvailablePaths(object resource);
}