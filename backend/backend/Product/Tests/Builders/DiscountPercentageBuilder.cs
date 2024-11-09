using AutoFixture.Kernel;
using backend.Product.DomainModels;

namespace backend.Product.Tests.Builders;

public class DiscountPercentageBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(DiscountPercentage)) return new NoSpecimen();
        var value = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 100m)) ?? 0m);
        return new DiscountPercentage(value);
    }
}