using backend.Shared.SqlFilter;
using SqlFilterBuilder = backend.Shared.SqlFilter.SqlFilterBuilder;

namespace backend.Supplier.Services;

public class SupplierSqlFilterBuilder(SqlFilterParser filterParser) : SqlFilterBuilder(filterParser);
