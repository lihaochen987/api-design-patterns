// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture.Kernel;
using backend.Review.DomainModels.ValueObjects;

namespace backend.Review.Tests.TestHelpers.SpecimenBuilders
{
    public class RatingSpecimenBuilder : ISpecimenBuilder
    {
        private readonly Random _random = new();

        public object Create(object request, ISpecimenContext context)
        {
            if (request is not Type type || type != typeof(Rating))
            {
                return new NoSpecimen();
            }

            decimal ratingValue = (decimal)(_random.NextDouble() * 4) + 1;

            return new Rating(ratingValue);
        }
    }
}
