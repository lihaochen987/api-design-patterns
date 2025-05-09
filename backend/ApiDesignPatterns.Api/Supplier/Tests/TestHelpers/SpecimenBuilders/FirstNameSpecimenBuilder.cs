// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture.Kernel;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.Tests.TestHelpers.SpecimenBuilders;

public class FirstNameSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(FirstName))
        {
            return new NoSpecimen();
        }

        var random = new Random();
        string[] validFirstNames =
        [
            "James", "Emma", "Michael", "Olivia", "William", "Sophia", "Alexander", "Charlotte", "Benjamin", "Ava",
            "Liam", "Isabella", "Noah", "Mia", "Ethan", "Amelia", "Daniel", "Harper", "Matthew", "Evelyn"
        ];

        string firstName = validFirstNames[random.Next(validFirstNames.Length)];

        return new FirstName(firstName);
    }
}
