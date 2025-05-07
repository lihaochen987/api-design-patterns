// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.Supplier.InfrastructureLayer.Database.Supplier;

namespace backend.Supplier.Tests.TestHelpers.Fakes;

public class SupplierRepositoryFake : Collection<DomainModels.Supplier>, ISupplierRepository
{
    public bool IsDirty { get; set; }

    public Task<DomainModels.Supplier?> GetSupplierAsync(long id)
    {
        DomainModels.Supplier? supplier = this.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(supplier);
    }

    public Task DeleteSupplierAsync(long id)
    {
        var supplier = this.FirstOrDefault(r => r.Id == id);
        if (supplier == null)
        {
            return Task.CompletedTask;
        }

        Remove(supplier);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task<long> CreateSupplierAsync(DomainModels.Supplier supplier)
    {
        IsDirty = true;
        Add(supplier);
        return Task.FromResult(supplier.Id);
    }

    public Task<long> UpdateSupplierAsync(DomainModels.Supplier supplier)
    {
        int index = IndexOf(this.FirstOrDefault(r => r.Id == supplier.Id) ??
                            throw new InvalidOperationException());
        this[index] = supplier;
        IsDirty = true;

        return Task.FromResult(supplier.Id);
    }

    public Task CreateSupplierAddressAsync(DomainModels.Supplier supplier)
    {
        IsDirty = true;
        var existingSupplier = this.FirstOrDefault(s => s.Id == supplier.Id);
        if (existingSupplier != null)
        {
            int index = IndexOf(existingSupplier);
            this[index] = supplier;
        }

        return Task.CompletedTask;
    }

    public Task CreateSupplierPhoneNumberAsync(DomainModels.Supplier supplier)
    {
        IsDirty = true;
        var existingSupplier = this.FirstOrDefault(s => s.Id == supplier.Id);
        if (existingSupplier != null)
        {
            int index = IndexOf(existingSupplier);
            this[index] = supplier;
        }

        return Task.CompletedTask;
    }

    public Task UpdateSupplierPhoneNumberAsync(DomainModels.Supplier supplier)
    {
        IsDirty = true;
        var existingSupplier = this.FirstOrDefault(s => s.Id == supplier.Id);
        if (existingSupplier != null)
        {
            int index = IndexOf(existingSupplier);
            this[index] = existingSupplier with { PhoneNumbers = supplier.PhoneNumbers };
        }

        return Task.CompletedTask;
    }

    public Task UpdateSupplierAddressAsync(DomainModels.Supplier supplier)
    {
        IsDirty = true;
        var existingSupplier = this.FirstOrDefault(s => s.Id == supplier.Id);
        if (existingSupplier != null)
        {
            int index = IndexOf(existingSupplier);
            this[index] = existingSupplier with { Addresses = supplier.Addresses };
        }

        return Task.CompletedTask;
    }
}
