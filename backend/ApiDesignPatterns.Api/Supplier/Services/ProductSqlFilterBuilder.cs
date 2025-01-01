using backend.Shared.SqlFilter;
using SqlFilterBuilder = backend.Shared.SqlFilter.SqlFilterBuilder;

namespace backend.Supplier.Services;

public class ProductSqlFilterBuilder(SqlFilterParser filterParser) : SqlFilterBuilder(filterParser);
