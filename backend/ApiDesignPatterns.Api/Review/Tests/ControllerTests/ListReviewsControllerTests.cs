// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Queries.ListReviews;
using backend.Review.DomainModels;
using backend.Review.ReviewControllers;
using backend.Review.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Review.Tests.ControllerTests;

public class ListReviewsControllerTests : ListReviewsControllerTestBase
{
    [Fact]
    public async Task ListReviews_ShouldReturnAllReviews_WhenNoPageTokenProvided()
    {
        string parentId = Fixture.Create<long>().ToString();
        List<ReviewView> reviewViews = new ReviewViewTestDataBuilder().CreateMany(4).ToList();
        ListReviewsRequest request = new() { MaxPageSize = 4 };
        Mock
            .Get(MockListReviews)
            .Setup(svc => svc.Handle(It.Is<ListReviewsQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.ParentId == parentId)))
            .ReturnsAsync(new PagedReviews(reviewViews, null));
        ListReviewsController sut = ListReviewsController();

        var result = await sut.ListReviews(request, parentId);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        var listReviewsResponse = (ListReviewsResponse)response.Value!;
        listReviewsResponse!.Results.Count().ShouldBe(4);
        listReviewsResponse.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task ListReviews_ShouldReturnReviewsAfterPageToken_WhenPageTokenProvided()
    {
        string parentId = Fixture.Create<long>().ToString();
        List<ReviewView> reviewViewList = new ReviewViewTestDataBuilder().CreateMany(4).ToList();
        var expectedPageResults = reviewViewList.Skip(2).Take(2).ToList();
        ListReviewsRequest request = new() { PageToken = "2", MaxPageSize = 2 };
        Mock
            .Get(MockListReviews)
            .Setup(svc => svc.Handle(It.Is<ListReviewsQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.ParentId == parentId)))
            .ReturnsAsync(new PagedReviews(expectedPageResults, null));
        ListReviewsController sut = ListReviewsController();

        var result = await sut.ListReviews(request, parentId);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        var listReviewsResponse = (ListReviewsResponse)response.Value!;
        listReviewsResponse!.Results.Count().ShouldBe(2);
        listReviewsResponse.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task ListReviews_ShouldReturnNextPageToken_WhenMoreReviewsExist()
    {
        string parentId = Fixture.Create<long>().ToString();
        List<ReviewView> reviews = new ReviewViewTestDataBuilder().CreateMany(20).ToList();
        List<ReviewView> firstPageReviews = reviews.Take(2).ToList();
        ListReviewsRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListReviews)
            .Setup(svc => svc.Handle(It.Is<ListReviewsQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.ParentId == parentId)))
            .ReturnsAsync(new PagedReviews(firstPageReviews, "2"));
        ListReviewsController sut = ListReviewsController();

        var result = await sut.ListReviews(request, parentId);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        var listReviewsResponse = (ListReviewsResponse)response.Value!;
        listReviewsResponse!.Results.Count().ShouldBe(2);
        listReviewsResponse.NextPageToken.ShouldBeEquivalentTo("2");
    }

    [Fact]
    public async Task ListReviews_ShouldUseDefaults_WhenPageTokenAndMaxPageSizeNotProvided()
    {
        string parentId = Fixture.Create<long>().ToString();
        List<ReviewView> reviews = new ReviewViewTestDataBuilder().CreateMany(20).ToList();
        List<ReviewView> defaultPageReviews = reviews.Take(DefaultMaxPageSize).ToList();
        ListReviewsRequest request = new();
        Mock
            .Get(MockListReviews)
            .Setup(svc => svc.Handle(It.Is<ListReviewsQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.ParentId == parentId)))
            .ReturnsAsync(new PagedReviews(defaultPageReviews, DefaultMaxPageSize.ToString()));
        ListReviewsController sut = ListReviewsController();

        var result = await sut.ListReviews(request, parentId);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        var listReviewsResponse = (ListReviewsResponse)response.Value!;
        listReviewsResponse!.Results.Count().ShouldBe(DefaultMaxPageSize);
        listReviewsResponse.NextPageToken.ShouldBeEquivalentTo(DefaultMaxPageSize.ToString());
    }

    [Fact]
    public async Task ListReviews_ShouldReturnEmptyList_WhenNoReviewsExist()
    {
        string parentId = Fixture.Create<long>().ToString();
        ListReviewsRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListReviews)
            .Setup(svc => svc.Handle(It.Is<ListReviewsQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.ParentId == parentId)))
            .ReturnsAsync(new PagedReviews([], null));
        ListReviewsController sut = ListReviewsController();

        var result = await sut.ListReviews(request, parentId);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.ShouldNotBeNull();
        var listReviewsResponse = (ListReviewsResponse)response.Value!;
        listReviewsResponse!.Results.ShouldBeEmpty();
        listReviewsResponse.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task ListReviews_WithFilterAndPagination_ReturnsCorrectResults()
    {
        string parentId = Fixture.Create<long>().ToString();
        var review = new ReviewViewTestDataBuilder()
            .WithId(2)
            .WithRating(5)
            .Build();
        var filteredReviews = new List<ReviewView> { review };
        ListReviewsRequest request = new() { Filter = "Rating == 5", MaxPageSize = 2, PageToken = "1" };
        Mock
            .Get(MockListReviews)
            .Setup(svc => svc.Handle(It.Is<ListReviewsQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken &&
                q.ParentId == parentId)))
            .ReturnsAsync(new PagedReviews(filteredReviews, "2"));
        ListReviewsController sut = ListReviewsController();

        var result = await sut.ListReviews(request, parentId);

        result.Result.ShouldBeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        var listReviewsResponse = (ListReviewsResponse)response.Value!;
        listReviewsResponse!.Results.Count().ShouldBe(1);
        listReviewsResponse.NextPageToken.ShouldBe("2");
    }
}
