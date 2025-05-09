// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture.Kernel;
using backend.PhoneNumber.DomainModels.ValueObjects;

namespace backend.PhoneNumber.Tests.TestHelpers.SpecimenBuilders;

public class PhoneDigitsSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(PhoneDigits))
        {
            return new NoSpecimen();
        }

        var random = new Random();
        int digitCount = random.Next(7, 9);

        long number = 0;
        for (int i = 0; i < digitCount; i++)
        {
            number = number * 10 + random.Next(0, 10);
        }

        return new PhoneDigits(number);
    }
}
