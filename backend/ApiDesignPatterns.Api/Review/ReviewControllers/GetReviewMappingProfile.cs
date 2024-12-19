// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Review.DomainModels.Views;

namespace backend.Review.ReviewControllers;

public class GetReviewMappingProfile : Profile
{
    public GetReviewMappingProfile()
    {
        CreateMap<ReviewView, GetReviewResponse>();
    }
}
