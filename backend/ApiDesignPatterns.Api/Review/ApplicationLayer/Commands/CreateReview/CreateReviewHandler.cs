// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer.Database.Review;
using backend.Shared.CommandHandler;

namespace backend.Review.ApplicationLayer.Commands.CreateReview;

public class CreateReviewHandler(IReviewRepository repository) : ICommandHandler<CreateReviewCommand>
{
    public async Task Handle(CreateReviewCommand command)
    {
        var review = command.Review with { ProductId = command.ProductId, CreatedAt = DateTimeOffset.UtcNow };
        await repository.CreateReviewAsync(review);
    }
}
