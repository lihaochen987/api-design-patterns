using Xunit;

namespace backend.Shared;

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<ProductRepositoryTests>;
