// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.Controllers;
using backend.Review.DomainModels.ValueObjects;
using backend.Review.InfrastructureLayer.Database.Review;
using backend.Shared.CommandHandler;

namespace backend.Review.ApplicationLayer.Commands.UpdateReview;

public class UpdateReviewHandler(IReviewRepository repository) : ICommandHandler<UpdateReviewCommand>
{
    public async Task Handle(UpdateReviewCommand command)
    {
        (long productId, Rating rating, Text text) = GetUpdatedReviewValues(command.Request, command.Review);
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
        Rating rating,
        Text text)
        GetUpdatedReviewValues(
            UpdateReviewRequest request,
            DomainModels.Review review)
    {
        long productId = request.FieldMask.Contains("productid") && !string.IsNullOrEmpty(request.ProductId)
            ? long.Parse(request.ProductId)
            : review.ProductId;

        Rating rating = request.FieldMask.Contains("rating", StringComparer.OrdinalIgnoreCase)
                         && request.Rating != null
            ? new Rating(request.Rating.Value)
            : review.Rating;

        Text text = request.FieldMask.Contains("text", StringComparer.OrdinalIgnoreCase)
                      && !string.IsNullOrEmpty(request.Text)
            ? new Text(request.Text)
            : review.Text;

        return (productId, rating, text);
    }
}
