using backend.Review.Controllers;
using backend.Review.DomainModels;
using Mapster;

namespace backend.Review.Services;

public static class ReviewMappingConfig
{
    public static void RegisterReviewMappings(this TypeAdapterConfig config)
    {
        config.NewConfig<DomainModels.Review, CreateReviewResponse>();
        config.NewConfig<ReviewView, GetReviewResponse>();
        config.NewConfig<DomainModels.Review, ReplaceReviewResponse>();
        config.NewConfig<ReplaceReviewRequest, DomainModels.Review>();
        config.NewConfig<DomainModels.Review, UpdateReviewResponse>();
        config.NewConfig<CreateReviewRequest, DomainModels.Review>();

        config.NewConfig<DomainModels.Review, CreateReviewRequest>();
        config.NewConfig<DomainModels.Review, ReplaceReviewRequest>();
    }
}
