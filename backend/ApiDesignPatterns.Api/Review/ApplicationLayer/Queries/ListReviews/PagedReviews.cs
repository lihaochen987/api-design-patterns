// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.DomainModels;

namespace backend.Review.ApplicationLayer.Queries.ListReviews;

public record PagedReviews(List<ReviewView> Reviews, string? NextPageToken);
