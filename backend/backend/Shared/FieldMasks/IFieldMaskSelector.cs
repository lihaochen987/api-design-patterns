namespace backend.Shared.FieldMasks;

public interface IFieldMaskSelector
{
    HashSet<string> ValidFields(
        IEnumerable<string> fields, 
        object targetObject);
}