// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer.Database.Review;
using backend.Shared.CommandHandler;

namespace backend.Review.ApplicationLayer.Commands.ReplaceReview;

public class ReplaceReviewHandler(IReviewRepository repository) : ICommandHandler<ReplaceReviewCommand>
{
    public async Task Handle(ReplaceReviewCommand command)
    {
        var review = command.Review with { CreatedAt = DateTimeOffset.Now, UpdatedAt = null };
        await repository.UpdateReviewAsync(review);
    }
}
