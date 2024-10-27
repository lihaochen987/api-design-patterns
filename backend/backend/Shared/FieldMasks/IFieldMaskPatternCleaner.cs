namespace backend.Shared.FieldMasks;

public interface IFieldMaskPatternCleaner
{
    string RemoveResponsePattern(string input);
    string RemoveContractPattern(string input);
}