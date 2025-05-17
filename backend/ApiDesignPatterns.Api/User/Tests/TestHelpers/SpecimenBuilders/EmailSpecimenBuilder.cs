// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture.Kernel;
using backend.User.DomainModels.ValueObjects;

namespace backend.User.Tests.TestHelpers.SpecimenBuilders;

public class EmailSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(Email))
        {
            return new NoSpecimen();
        }

        var random = new Random();
        string[] validEmails =
        [
            "user1@example.com",
            "contact@test.org",
            "john.doe@email.net",
            "support@company.co",
            "info@domain.io",
            "sales@business.biz",
            "help@service.com",
            "admin@system.net",
            "team@project.org",
            "office@work.com"
        ];

        string email = validEmails[random.Next(validEmails.Length)];

        return new Email(email);
    }
}
