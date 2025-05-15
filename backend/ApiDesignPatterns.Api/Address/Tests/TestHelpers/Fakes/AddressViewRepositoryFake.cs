// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.Address.DomainModels;
using backend.Address.InfrastructureLayer.Database.AddressView;

namespace backend.Address.Tests.TestHelpers.Fakes;

public class AddressViewRepositoryFake
    : Collection<AddressView>, IAddressViewRepository
{
    public Task<AddressView?> GetAddressViewAsync(long id)
    {
        var addressView = this.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(addressView);
    }

    public Task<(List<AddressView>, string?)> ListAddressAsync(string? pageToken, string? filter, int maxPageSize) =>
        throw new NotImplementedException();
}
