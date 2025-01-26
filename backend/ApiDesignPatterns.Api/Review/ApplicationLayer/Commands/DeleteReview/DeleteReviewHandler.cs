// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer.Database.Review;
using backend.Shared.CommandHandler;

namespace backend.Review.ApplicationLayer.Commands.DeleteReview;

public class DeleteReviewHandler(IReviewRepository repository) : ICommandHandler<DeleteReviewQuery>
{
    public async Task Handle(DeleteReviewQuery command)
    {
        await repository.DeleteReviewAsync(command.Id);
    }
}
