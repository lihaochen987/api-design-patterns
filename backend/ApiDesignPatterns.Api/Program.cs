using System.Data;
using System.Text.Json.Serialization;
using backend.Product;
using backend.Review;
using backend.Shared;
using backend.Supplier;
using DbUp;
using DbUp.Engine;
using Npgsql;
using SqlFilterBuilder = backend.Shared.SqlFilterBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsPolicyBuilder => corsPolicyBuilder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Inject shared classes
builder.Services.AddSingleton<TypeParser>();
builder.Services.AddSingleton<RequireNonNullablePropertiesSchemaFilter>();
builder.Services.AddSingleton<SqlFilterBuilder>(provider =>
    new SqlFilterBuilder(
        provider.GetRequiredService<IColumnMapper>()));

// Todo: Refactor this out once 1. we convert the Product resource to use Dapper
var recursiveValidator = new RecursiveValidator();

var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

// Review Composition Root
var reviewCompositionRoot =
    new ReviewComposer(
        builder.Configuration,
        loggerFactory);
reviewCompositionRoot.ConfigureServices(builder.Services);

// Product Composition Root
var productCompositionRoot =
    new ProductComposer(
        builder.Configuration,
        loggerFactory,
        recursiveValidator);
productCompositionRoot.ConfigureServices(builder.Services);

builder.Services.AddSupplierDependencies();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.UseOneOfForPolymorphism();
    c.SchemaFilter<RequireNonNullablePropertiesSchemaFilter>();
});

// Register Dapper stuff
builder.Services.AddScoped<IDbConnection>(_ =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    string? connectionString = configuration.GetConnectionString("DefaultConnection");

    try
    {
        string migrationPath = Path.Combine(Directory.GetCurrentDirectory(), "UpScripts");

        if (connectionString != null && Directory.Exists(migrationPath))
        {
            ApplyMigrations(connectionString, 5, 2000, migrationPath);
        }
        else
        {
            Console.WriteLine("Invalid connection string or migration path.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
return;

void ApplyMigrations(
    string connectionString,
    int retryCount,
    int delayInSeconds,
    string migrationPath)
{
    if (!Directory.Exists(migrationPath))
    {
        Console.WriteLine($"Migration path does not exist: {migrationPath}");
        return;
    }

    for (int attempt = 1; attempt <= retryCount; attempt++)
    {
        try
        {
            EnsureDatabase.For.PostgresqlDatabase(connectionString);
            UpgradeEngine? upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsFromFileSystem(migrationPath)
                .LogToConsole()
                .Build();

            DatabaseUpgradeResult? result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                throw new Exception("Migration failed: " + result.Error);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All migrations applied successfully.");
            Console.ResetColor();
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");
            if (attempt == retryCount)
            {
                throw;
            }

            Console.WriteLine("Retrying in 2 seconds...");
            Thread.Sleep(delayInSeconds);
        }
    }
}
