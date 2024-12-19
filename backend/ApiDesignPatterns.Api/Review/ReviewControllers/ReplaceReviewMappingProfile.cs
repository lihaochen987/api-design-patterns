// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;

namespace backend.Review.ReviewControllers;

public class ReplaceReviewMappingProfile : Profile
{
    public ReplaceReviewMappingProfile()
    {
        CreateMap<DomainModels.Review, ReplaceReviewResponse>();
        CreateMap<ReplaceReviewRequest, DomainModels.Review>();
    }
}
