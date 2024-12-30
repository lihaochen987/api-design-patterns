// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.Services;
using backend.Product.Tests.TestHelpers.Builders;

namespace backend.Product.Tests.ServiceTests;

public class ProductFieldMaskConfigurationTestBase
{
    protected readonly IFixture Fixture;

    protected ProductFieldMaskConfigurationTestBase()
    {
        Fixture = new Fixture();
        Fixture.Customizations.Add(new ProductDimensionsBuilder());
    }

    protected ProductFieldMaskConfiguration ProductFieldMaskConfiguration()
    {
        return new ProductFieldMaskConfiguration();
    }
}
