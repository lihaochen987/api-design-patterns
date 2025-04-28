// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Review.Controllers;
using backend.Review.DomainModels;
using backend.Review.DomainModels.ValueObjects;

namespace backend.Review.Services;

public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<DomainModels.Review, CreateReviewResponse>();
        CreateMap<ReviewView, GetReviewResponse>();
        CreateMap<DomainModels.Review, ReplaceReviewResponse>();
        CreateMap<ReplaceReviewRequest, DomainModels.Review>();
        CreateMap<DomainModels.Review, UpdateReviewResponse>();
        CreateMap<CreateReviewRequest, DomainModels.Review>();

        CreateMap<DomainModels.Review, CreateReviewRequest>();
        CreateMap<DomainModels.Review, ReplaceReviewRequest>();

        CreateMap<Rating, decimal>().ConvertUsing(src => src.Value);
        CreateMap<decimal, Rating>().ConvertUsing(src => new Rating(src));
    }
}
