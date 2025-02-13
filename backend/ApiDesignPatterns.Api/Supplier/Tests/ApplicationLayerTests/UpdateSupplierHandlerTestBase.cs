// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;
using backend.Supplier.Tests.TestHelpers.Fakes;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public abstract class UpdateSupplierHandlerTestBase
{
    protected readonly SupplierRepositoryFake Repository = [];

    protected ICommandHandler<UpdateSupplierCommand> UpdateSupplierHandler()
    {
        return new UpdateSupplierHandler(Repository);
    }
}
