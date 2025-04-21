// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Queries.GetReviewView;
using backend.Review.DomainModels;
using backend.Review.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Review.Tests.ApplicationLayerTests;

public abstract class GetReviewViewHandlerTestBase
{
    protected readonly ReviewViewRepositoryFake Repository = new(new PaginateService<ReviewView>());

    protected IAsyncQueryHandler<GetReviewViewQuery, ReviewView?> GetReviewViewHandler()
    {
        return new GetReviewViewHandler(Repository);
    }
}
