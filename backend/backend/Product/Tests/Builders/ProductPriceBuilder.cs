using System.Reflection;
using AutoFixture.Kernel;

namespace backend.Product.Tests.Builders;

public class ProductPriceBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        var pi = request as PropertyInfo;
        if (pi == null ||
            pi.Name != "DiscountPercentage" ||
            pi.PropertyType != typeof(decimal))
            return new NoSpecimen();

        return context.Resolve(
            new RangedNumberRequest(typeof(decimal), 0.0m, 1.0m));
    }
}