// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.User.DomainModels;

namespace backend.User.ApplicationLayer.Queries.ListUsers;

public record PagedUsers(List<UserView> Users, string? NextPageToken);
