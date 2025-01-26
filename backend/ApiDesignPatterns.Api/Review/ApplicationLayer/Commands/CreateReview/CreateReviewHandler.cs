// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer.Database.Review;
using backend.Shared.CommandHandler;

namespace backend.Review.ApplicationLayer.Commands.CreateReview;

public class CreateReviewHandler(IReviewRepository repository) : ICommandHandler<CreateReviewQuery>
{
    public async Task Handle(CreateReviewQuery command)
    {
        await repository.CreateReviewAsync(command.Review);
    }
}
