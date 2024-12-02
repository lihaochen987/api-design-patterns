using AutoFixture.Kernel;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.Tests.Helpers.Builders;

public class ProductPricingBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(Pricing))
        {
            return new NoSpecimen();
        }

        decimal basePrice = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0.01m, 1000m)) ?? 1m);
        decimal discountPercentage =
            (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 100m)) ?? 0m);
        decimal taxRate = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 100m)) ?? 0m);

        return new Pricing(basePrice, discountPercentage, taxRate);
    }
}
