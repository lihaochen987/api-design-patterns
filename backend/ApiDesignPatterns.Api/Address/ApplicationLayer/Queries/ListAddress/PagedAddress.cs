// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.DomainModels;

namespace backend.Address.ApplicationLayer.Queries.ListAddress;

public record PagedAddress(List<AddressView> Address, string? NextPageToken);
