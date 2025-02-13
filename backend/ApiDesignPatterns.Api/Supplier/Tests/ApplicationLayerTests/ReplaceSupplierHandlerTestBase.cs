// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.ReplaceSupplier;
using backend.Supplier.Tests.TestHelpers.Fakes;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public abstract class ReplaceSupplierHandlerTestBase
{
    protected readonly SupplierRepositoryFake Repository = [];
    protected readonly Fixture Fixture = new();

    protected ICommandHandler<ReplaceSupplierCommand> ReplaceSupplierHandler()
    {
        return new ReplaceSupplierHandler(Repository);
    }
}
