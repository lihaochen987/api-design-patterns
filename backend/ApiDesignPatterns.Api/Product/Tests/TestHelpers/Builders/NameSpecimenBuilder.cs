// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.Tests.TestHelpers.Builders;

using AutoFixture.Kernel;
using DomainModels.ValueObjects;
using System;
using System.Text;

public class NameSpecimenBuilder : ISpecimenBuilder
{
    private readonly Random _random = new();

    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(Name))
        {
            return new NoSpecimen();
        }

        int length = _random.Next(1, 50);

        var sb = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            char c = (char)(_random.Next(26) + 'a');
            sb.Append(c);
        }

        if (sb.Length > 0)
        {
            sb[0] = char.ToUpper(sb[0]);
        }

        return new Name(sb.ToString());
    }
}
