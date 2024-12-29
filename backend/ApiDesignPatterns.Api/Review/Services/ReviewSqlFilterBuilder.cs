using backend.Shared;

namespace backend.Review.Services;

public class ReviewSqlFilterBuilder() : SqlFilterBuilder(new ReviewColumnMapper());
