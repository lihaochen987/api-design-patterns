// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture.Kernel;
using backend.Address.DomainModels.ValueObjects;

namespace backend.Address.Tests.TestHelpers.SpecimenBuilders;

public class CountrySpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(Country))
        {
            return new NoSpecimen();
        }

        var random = new Random();
        string[] validCountries =
        {
            "United States", "Canada", "United Kingdom", "France", "Germany", "Japan", "Australia", "Brazil",
            "India", "South Africa"
        };

        string countryName = validCountries[random.Next(validCountries.Length)];

        return new Country(countryName);
    }
}
