using AutoFixture.Kernel;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.Tests.TestHelpers.SpecimenBuilders;

/// <summary>
/// Specimen builder for generating valid Pricing value objects that respect business invariants.
/// </summary>
public class ProductPricingSpecimenBuilder : ISpecimenBuilder
{
    private readonly Random _random = new();

    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(Pricing))
        {
            return new NoSpecimen();
        }

        bool isHighValue = _random.Next(2) == 1;

        if (isHighValue)
        {
            // High-value product (>1000)
            decimal basePrice =
                (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 1001m, 3000m)) ?? 1500m);

            // Keep discount modest to stay within 30% effective discount rule
            decimal discountPercentage =
                (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 20m)) ?? 10m);
            decimal taxRate = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 5m, 15m)) ?? 10m);

            return new Pricing(basePrice, discountPercentage, taxRate);
        }
        else
        {
            // Standard-value product (≤1000)
            decimal basePrice =
                (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 10m, 1000m)) ?? 500m);

            // Since cost is 60% of base price, maximum safe discount is ~25%
            decimal discountPercentage =
                (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 20m)) ?? 10m);
            decimal taxRate = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 15m)) ?? 8m);

            return new Pricing(basePrice, discountPercentage, taxRate);
        }
    }
}
