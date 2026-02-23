// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer.Database.Review;
using backend.Shared.CommandHandler;

namespace backend.Review.ApplicationLayer.Commands.ReplaceReview;

public class ReplaceReviewHandler(IReviewRepository repository) : ICommandHandler<ReplaceReviewCommand>
{
    public async Task Handle(ReplaceReviewCommand command)
    {
        var review = new DomainModels.Review(command.Review.Id, command.Review.ProductId, command.Review.Rating,
            command.Review.Text, DateTimeOffset.Now, null);
        await repository.UpdateReviewAsync(review);
    }
}
