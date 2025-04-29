// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using AutoFixture.Kernel;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.Tests.TestHelpers.SpecimenBuilders;

public class CountryCodeSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(CountryCode))
        {
            return new NoSpecimen();
        }

        var random = new Random();
        int digitCount = random.Next(1, 4);
        string code = "+";

        for (int i = 0; i < digitCount; i++)
        {
            code += random.Next(0, 10).ToString();
        }

        return new CountryCode(code);
    }
}
