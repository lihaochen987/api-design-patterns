// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer.Database.Review;
using backend.Review.ReviewControllers;
using backend.Shared.CommandHandler;

namespace backend.Review.ApplicationLayer.Commands.UpdateReview;

public class UpdateReviewHandler(IReviewRepository repository) : ICommandHandler<UpdateReviewCommand>
{
    public async Task Handle(UpdateReviewCommand command)
    {
        (long productId, decimal rating, string text) = GetUpdatedReviewValues(command.Request, command.Review);
        var review = new DomainModels.Review
        {
            Id = command.Review.Id,
            ProductId = productId,
            Rating = rating,
            Text = text,
            CreatedAt = command.Review.CreatedAt,
            UpdatedAt = DateTimeOffset.Now
        };
        await repository.UpdateReviewAsync(review);
    }

    private static (
        long productId,
        decimal rating,
        string text)
        GetUpdatedReviewValues(
            UpdateReviewRequest request,
            DomainModels.Review review)
    {
        long productId = request.FieldMask.Contains("productid") && !string.IsNullOrEmpty(request.ProductId)
            ? long.Parse(request.ProductId)
            : review.ProductId;

        decimal rating = request.FieldMask.Contains("rating", StringComparer.OrdinalIgnoreCase)
                         && !string.IsNullOrEmpty(request.Rating)
            ? decimal.Parse(request.Rating)
            : review.Rating;

        string text = request.FieldMask.Contains("text", StringComparer.OrdinalIgnoreCase)
                      && !string.IsNullOrEmpty(request.Text)
            ? request.Text
            : review.Text;

        return (productId, rating, text);
    }
}
