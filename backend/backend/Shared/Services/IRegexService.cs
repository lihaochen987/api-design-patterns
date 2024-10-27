namespace backend.Shared.Services;

public interface IRegexService
{
    string RemoveHcLc(string input);
    string RemoveContractFromString(string input);
}