// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.CreateSupplier;
using backend.Supplier.Tests.TestHelpers;
using backend.Supplier.Tests.TestHelpers.Fakes;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public abstract class CreateSupplierHandlerTestBase
{
    protected readonly Fixture Fixture = new();
    protected readonly SupplierRepositoryFake Repository = [];

    protected ICommandHandler<CreateSupplierCommand> CreateSupplierService()
    {
        return new CreateSupplierHandler(Repository);
    }
}
