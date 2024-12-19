namespace backend.Product.DomainModels.ValueObjects;

public record Dimensions
{
    private readonly decimal _length;
    private readonly decimal _width;
    private readonly decimal _height;

    public decimal Length
    {
        get => _length;
        init
        {
            if (value is < 0 or > 100)
            {
                throw new ArgumentException("Length must be greater than zero and less than 100cm.");
            }

            _length = value;
            EnforceInvariants();
        }
    }

    public decimal Width
    {
        get => _width;
        init
        {
            if (value is < 0 or > 50)
            {
                throw new ArgumentException("Width must be greater than zero and less than 50cm.");
            }

            _width = value;
            EnforceInvariants();
        }
    }

    public decimal Height
    {
        get => _height;
        init
        {
            if (value is < 0 or > 50)
            {
                throw new ArgumentException("Height must be greater than zero and less than 50cm.");
            }

            _height = value;
            EnforceInvariants();
        }
    }

    private void EnforceInvariants()
    {
        if (_width * _length * _height > 110000)
        {
            throw new ArgumentException("Total volume must be less than 110,000cm³.");
        }
    }
}
