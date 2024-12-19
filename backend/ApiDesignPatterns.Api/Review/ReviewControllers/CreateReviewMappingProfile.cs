// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;

namespace backend.Review.ReviewControllers;

public class CreateReviewMappingProfile : Profile
{
    public CreateReviewMappingProfile()
    {
        CreateMap<DomainModels.Review, CreateReviewResponse>();
    }
}
