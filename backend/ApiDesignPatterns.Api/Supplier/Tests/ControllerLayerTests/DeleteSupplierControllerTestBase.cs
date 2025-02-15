// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Commands.DeleteSupplier;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.SupplierControllers;
using Moq;

namespace backend.Supplier.Tests.ControllerLayerTests;

public abstract class DeleteSupplierControllerTestBase
{
    protected readonly IQueryHandler<GetSupplierQuery, DomainModels.Supplier> MockGetSupplierHandler =
        Mock.Of<IQueryHandler<GetSupplierQuery, DomainModels.Supplier>>();

    protected readonly ICommandHandler<DeleteSupplierCommand> MockDeleteSupplierHandler =
        Mock.Of<ICommandHandler<DeleteSupplierCommand>>();

    protected DeleteSupplierController DeleteSupplierController()
    {
        return new DeleteSupplierController(MockGetSupplierHandler, MockDeleteSupplierHandler);
    }
}
