namespace backend.Shared.Services;

public interface IFieldPreparationService
{
    HashSet<string> PrepareFields(IEnumerable<string> fields, object targetObject);
}