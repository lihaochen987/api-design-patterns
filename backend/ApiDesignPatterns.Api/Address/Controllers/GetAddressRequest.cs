namespace backend.Address.Controllers;

public class GetAddressRequest
{
    public List<string> FieldMask { get; set; } = ["*"];
}
