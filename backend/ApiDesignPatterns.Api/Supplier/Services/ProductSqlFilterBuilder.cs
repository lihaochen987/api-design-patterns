using backend.Shared;

namespace backend.Supplier.Services;

public class ProductSqlFilterBuilder() : SqlFilterBuilder(new ProductColumnMapper());
