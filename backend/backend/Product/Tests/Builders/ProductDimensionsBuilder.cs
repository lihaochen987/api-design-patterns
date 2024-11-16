using AutoFixture.Kernel;
using backend.Product.DomainModels;

namespace backend.Product.Tests.Builders;

public class ProductDimensionsBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(Dimensions))
            return new NoSpecimen();

        decimal length, width, height;

        do
        {
            length = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 100m)) ?? 0m);
            width = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 50m)) ?? 0m);
            height = (decimal)(context.Resolve(new RangedNumberRequest(typeof(decimal), 0m, 50m)) ?? 0m);
        } while (length * width * height >= 110_000m);

        return new Dimensions(length, width, height);
    }
}