// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Review.DomainModels.Views;

namespace backend.Review.ReviewControllers;

public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<DomainModels.Review, CreateReviewResponse>();
        CreateMap<ReviewView, GetReviewResponse>();
        CreateMap<DomainModels.Review, ReplaceReviewResponse>();
        CreateMap<ReplaceReviewRequest, DomainModels.Review>();
        CreateMap<DomainModels.Review, UpdateReviewResponse>();
    }
}
