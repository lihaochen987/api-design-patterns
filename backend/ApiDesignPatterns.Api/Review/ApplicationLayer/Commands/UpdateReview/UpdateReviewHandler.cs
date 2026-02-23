// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer.Database.Review;
using backend.Shared.CommandHandler;

namespace backend.Review.ApplicationLayer.Commands.UpdateReview;

public class UpdateReviewHandler(IReviewRepository repository) : ICommandHandler<UpdateReviewCommand>
{
    public async Task Handle(UpdateReviewCommand command)
    {
        var req = command.Request;
        var current = command.Review;

        long productId = req.FieldMask.Contains("productid") && !string.IsNullOrEmpty(req.ProductId)
            ? long.Parse(req.ProductId)
            : current.ProductId;

        decimal rating = req.FieldMask.Contains("rating", StringComparer.OrdinalIgnoreCase) && req.Rating != null
            ? req.Rating.Value
            : current.Rating;

        string text = req.FieldMask.Contains("text", StringComparer.OrdinalIgnoreCase) &&
                      !string.IsNullOrEmpty(req.Text)
            ? req.Text
            : current.Text;

        var updatedReview =
            new DomainModels.Review(current.Id, productId, rating, text, current.CreatedAt, DateTimeOffset.Now);

        await repository.UpdateReviewAsync(updatedReview);
    }
}
