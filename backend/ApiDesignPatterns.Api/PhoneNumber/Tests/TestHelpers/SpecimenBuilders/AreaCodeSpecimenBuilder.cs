// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture.Kernel;
using backend.PhoneNumber.DomainModels.ValueObjects;

namespace backend.PhoneNumber.Tests.TestHelpers.SpecimenBuilders;

public class AreaCodeSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(AreaCode))
        {
            return new NoSpecimen();
        }

        var random = new Random();
        int digitCount = random.Next(2, 6);
        string code = "";

        for (int i = 0; i < digitCount; i++)
        {
            code += random.Next(0, 10).ToString();
        }

        return new AreaCode(code);
    }
}
