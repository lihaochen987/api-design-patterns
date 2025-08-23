// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.Address.DomainModels;
using backend.Address.InfrastructureLayer.Database.AddressView;
using backend.Address.Tests.TestHelpers.Builders;
using backend.Shared;

namespace backend.Address.Tests.TestHelpers.Fakes;

public class AddressViewRepositoryFake(PaginateService<AddressView> paginateService)
    : Collection<AddressView>, IGetAddressView, IListAddressView
{
    public void AddAddressView(long userId, string fullAddress)
    {
        var addressView = new AddressViewTestDataBuilder()
            .WithUserId(userId)
            .WithFullAddress(fullAddress)
            .Build();
        Add(addressView);
    }

    public Task<AddressView?> GetAddressViewAsync(long id)
    {
        var addressView = this.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(addressView);
    }

    public Task<(List<AddressView>, string?)> ListAddressAsync(string? pageToken, string? filter, int maxPageSize)
    {
        var query = this.AsEnumerable();

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenAddress))
        {
            query = query.Where(r => r.Id > lastSeenAddress);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            if (filter.Contains("UserId =="))
            {
                string value = filter.Split("==")[1].Trim();
                query = query.Where(s => s.UserId == decimal.Parse(value));
            }
            else if (filter.Contains("FullAddress.Contains("))
            {
                int startIndex = filter.IndexOf('\'') + 1;
                int endIndex = filter.LastIndexOf('\'');
                if (startIndex > 0 && endIndex > startIndex)
                {
                    string searchValue = filter.Substring(startIndex, endIndex - startIndex);
                    query = query.Where(a => a.FullAddress.Contains(searchValue));
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        var addresses = query
            .OrderBy(r => r.Id)
            .Take(maxPageSize + 1)
            .ToList();

        var paginatedAddresses =
            paginateService.Paginate(addresses, maxPageSize, out string? nextPageToken);

        return Task.FromResult((paginatedAddresses, nextPageToken));
    }
}
