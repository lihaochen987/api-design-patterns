using AutoFixture.Kernel;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.Tests.Builders;

public class ProductPricingBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(Pricing))
            return new NoSpecimen();

        var basePrice = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0.01m, 1000m)) ?? 1m);
        var discountPercentage = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 100m)) ?? 0m);
        var taxRate = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 100m)) ?? 0m);

        return new Pricing(basePrice, discountPercentage, taxRate);
    }
}