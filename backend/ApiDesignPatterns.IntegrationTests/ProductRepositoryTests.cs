using System.Data;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.Tests.TestHelpers.Fakes;
using DbUp;
using FluentAssertions;
using Npgsql;
using Testcontainers.PostgreSql;

namespace IntegrationTests;

public class ProductRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:14")
        .WithPortBinding(5432, true)
        .WithDatabase("test_products_db")
        .WithUsername("test_user")
        .WithPassword("test_pass")
        .Build();

    private IProductRepository _repository = new ProductRepositoryFake();
    private string _connectionString = "";

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _connectionString = _dbContainer.GetConnectionString();
        string projectDir = FindProjectRoot(Directory.GetCurrentDirectory());
        string upScriptsPath = Path.Combine(projectDir, "UpScripts");
        ApplyMigrationsWithPath(upScriptsPath);
        IDbConnection connection = new NpgsqlConnection(_connectionString);
        _repository = new ProductRepository(connection);
    }

    private void ApplyMigrationsWithPath(string scriptsPath)
    {
        if (!Directory.Exists(scriptsPath))
        {
            throw new DirectoryNotFoundException($"Scripts directory not found: {scriptsPath}");
        }

        EnsureDatabase.For.PostgresqlDatabase(_connectionString);
        var upgrader = DeployChanges.To
            .PostgresqlDatabase(_connectionString)
            .WithScriptsFromFileSystem(scriptsPath)
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();
        if (!result.Successful)
        {
            throw new Exception("Migration failed: " + result.Error);
        }
    }

    private static string FindProjectRoot(string startDir)
    {
        string? directory = startDir;

        while (!string.IsNullOrEmpty(directory))
        {
            string? fullName = Directory.GetParent(directory)?.FullName;
            if (fullName != null)
            {
                string apiProjectDir = Path.Combine(
                    fullName,
                    "ApiDesignPatterns.Api");

                if (Directory.Exists(apiProjectDir))
                {
                    string upScriptsDir = Path.Combine(apiProjectDir, "UpScripts");
                    if (Directory.Exists(upScriptsDir))
                    {
                        return apiProjectDir;
                    }
                }
            }

            directory = Directory.GetParent(directory)?.FullName;

            if (directory == null ||
                Path.GetPathRoot(directory) == directory)
            {
                break;
            }
        }

        throw new DirectoryNotFoundException("Could not find ApiDesignPatterns.Api directory with UpScripts");
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_ShouldReturnNull()
    {
        var product = await _repository.GetProductAsync(999999);

        product.Should().BeNull();
    }
}
