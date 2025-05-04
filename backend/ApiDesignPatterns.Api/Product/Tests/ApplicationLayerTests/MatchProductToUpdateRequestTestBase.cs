// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.MatchProductToUpdateRequest;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public class MatchProductToUpdateRequestTestBase
{
    protected Fixture Fixture = new();

    protected ISyncQueryHandler<MatchProductToUpdateRequestQuery, MatchProductToUpdateRequestResult>
        GetMatchProductToUpdateRequestHandler()
    {
        return new MatchProductToUpdateRequestHandler();
    }
}
