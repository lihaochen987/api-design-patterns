// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.Address.InfrastructureLayer.Database.Address;

namespace backend.Address.Tests.TestHelpers.Fakes;

public class AddressRepositoryFake : Collection<DomainModels.Address>, IGetAddress, IDeleteAddress, IUpdateAddress
{
    public bool IsDirty { get; set; }

    public Task<DomainModels.Address?> GetAddress(long id)
    {
        var address = this.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(address);
    }

    public Task DeleteAddress(long id)
    {
        var address = this.FirstOrDefault(r => r.Id == id);
        if (address == null)
        {
            return Task.CompletedTask;
        }

        Remove(address);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task UpdateAddressAsync(DomainModels.Address address)
    {
        int index = IndexOf(this.FirstOrDefault(r => r.Id == address.Id) ??
                            throw new InvalidOperationException());
        this[index] = address;
        IsDirty = true;

        return Task.CompletedTask;
    }
}
