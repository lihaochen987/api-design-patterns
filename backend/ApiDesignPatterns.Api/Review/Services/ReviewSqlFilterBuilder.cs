using backend.Shared.SqlFilter;
using SqlFilterBuilder = backend.Shared.SqlFilter.SqlFilterBuilder;

namespace backend.Review.Services;

public class ReviewSqlFilterBuilder(SqlFilterParser filterParser) : SqlFilterBuilder(filterParser);
