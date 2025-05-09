// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture.Kernel;
using backend.Address.DomainModels.ValueObjects;

namespace backend.Supplier.Tests.TestHelpers.SpecimenBuilders;

public class CitySpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(City))
        {
            return new NoSpecimen();
        }

        var random = new Random();
        string[] validCities = ["London", "Paris", "Berlin", "Tokyo", "Sydney", "Rome", "Madrid", "Oslo"];

        string cityName = validCities[random.Next(validCities.Length)];

        return new City(cityName);
    }
}
