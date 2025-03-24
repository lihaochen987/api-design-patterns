// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Queries.ListReviews;
using backend.Review.ReviewControllers;
using backend.Shared.QueryHandler;
using FluentAssertions;
using Xunit;

namespace backend.Review.Tests.ApplicationLayerTests;

public class ListReviewsHandlerTests : ListReviewsHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnReviewsAndNextPageToken()
    {
        const long productId = 123;
        var request = new ListReviewsRequest { Filter = "Rating >= 4", MaxPageSize = 5 };
        Repository.AddReviewView(productId, 5);
        Repository.AddReviewView(productId, 4);
        IQueryHandler<ListReviewsQuery, PagedReviews> sut = ListReviewsViewHandler();

        var result = await sut.Handle(new ListReviewsQuery { Request = request, ParentId = productId.ToString() });

        result.Should().NotBeNull();
        result.Reviews.Should().NotBeEmpty();
        result.Reviews.Should().HaveCount(2);
        result.NextPageToken.Should().BeNull();
        result.Reviews.Should().OnlyContain(r => r.ProductId == productId);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoReviewsExistForProduct()
    {
        long productId = Fixture.Create<long>();
        var request = new ListReviewsRequest();
        IQueryHandler<ListReviewsQuery, PagedReviews> sut = ListReviewsViewHandler();

        var result = await sut.Handle(new ListReviewsQuery { Request = request, ParentId = productId.ToString() });

        result.Should().NotBeNull();
        result.Reviews.Should().BeEmpty();
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnOnlyReviewsForSpecificProduct()
    {
        long productId = Fixture.Create<long>();
        var request = new ListReviewsRequest { MaxPageSize = 5 };
        Repository.AddReviewView(productId);
        Repository.AddReviewView(Fixture.Create<long>());
        IQueryHandler<ListReviewsQuery, PagedReviews> sut = ListReviewsViewHandler();

        var result = await sut.Handle(new ListReviewsQuery { Request = request, ParentId = productId.ToString() });

        result.Should().NotBeNull();
        result.Reviews.Should().HaveCount(1);
        result.Reviews.Should().OnlyContain(r => r.ProductId == productId);
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenRepositoryFails()
    {
        long productId = Fixture.Create<long>();
        var request = new ListReviewsRequest
        {
            PageToken = "1", Filter = "InvalidFilter == \"SomeValue\"", MaxPageSize = 5
        };
        IQueryHandler<ListReviewsQuery, PagedReviews> sut = ListReviewsViewHandler();

        await FluentActions.Invoking(() =>
            sut.Handle(new ListReviewsQuery { Request = request, ParentId = productId.ToString() })
        ).Should().ThrowAsync<ArgumentException>();
    }
}
