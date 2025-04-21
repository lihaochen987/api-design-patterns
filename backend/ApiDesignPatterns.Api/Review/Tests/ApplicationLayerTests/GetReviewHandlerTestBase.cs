// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.Tests.TestHelpers.Fakes;
using backend.Shared.QueryHandler;

namespace backend.Review.Tests.ApplicationLayerTests;

public abstract class GetReviewHandlerTestBase
{
    protected readonly ReviewRepositoryFake Repository = [];

    protected IAsyncQueryHandler<GetReviewQuery, DomainModels.Review?> GetReviewHandler()
    {
        return new GetReviewHandler(Repository);
    }
}
