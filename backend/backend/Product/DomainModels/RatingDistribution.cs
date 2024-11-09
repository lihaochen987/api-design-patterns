namespace backend.Product.DomainModels;

public class RatingDistribution
{
    public int FiveStars { get; private set; }
    public int FourStars { get; private set; }
    public int ThreeStars { get; private set; }
    public int TwoStars { get; private set; }
    public int OneStar { get; private set; }
}