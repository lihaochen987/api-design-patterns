// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer.Database.Review;
using backend.Shared.CommandHandler;

namespace backend.Review.ApplicationLayer.Commands.ReplaceReview;

public class ReplaceReviewHandler(IReviewRepository repository) : ICommandHandler<ReplaceReviewQuery>
{
    public async Task Handle(ReplaceReviewQuery command)
    {
        await repository.UpdateReviewAsync(command.Review);
    }
}
