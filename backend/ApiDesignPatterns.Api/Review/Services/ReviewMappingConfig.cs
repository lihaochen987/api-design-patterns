using backend.Review.Controllers;
using backend.Review.DomainModels;
using backend.Review.DomainModels.ValueObjects;
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

        config.NewConfig<Rating, decimal>()
            .MapWith(src => src.Value);
        config.NewConfig<decimal, Rating>()
            .MapWith(src => new Rating(src));
        config.NewConfig<string, Text>()
            .MapWith(src => new Text(src));
        config.NewConfig<Text, string>()
            .MapWith(src => src.Value);
    }
}
