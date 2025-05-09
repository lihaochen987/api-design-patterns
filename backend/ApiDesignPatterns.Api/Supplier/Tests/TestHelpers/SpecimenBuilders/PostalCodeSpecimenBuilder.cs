// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture.Kernel;
using backend.Address.DomainModels.ValueObjects;

namespace backend.Supplier.Tests.TestHelpers.SpecimenBuilders;

public class PostalCodeSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(PostalCode))
        {
            return new NoSpecimen();
        }

        var random = new Random();

        int length = random.Next(3, 10);

        char[] chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            if (random.Next(10) < 7)
            {
                chars[i] = (char)('0' + random.Next(10));
            }
            else
            {
                chars[i] = (char)('A' + random.Next(26));
            }
        }

        string postalCode = new(chars);
        return new PostalCode(postalCode);
    }
}
