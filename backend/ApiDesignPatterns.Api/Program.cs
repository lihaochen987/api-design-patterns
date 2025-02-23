using System.Data;
using System.Text.Json.Serialization;
using backend.Inventory;
using backend.Product;
using backend.Product.ProductControllers;
using backend.Review;
using backend.Shared;
using backend.Shared.ControllerActivators;
using backend.Supplier;
using DbUp;
using DbUp.Engine;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Controllers;
using Npgsql;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Allow CORS to frontend application
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsPolicyBuilder => corsPolicyBuilder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add caching
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024;
    options.CompactionPercentage = 0.1;
});

var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

// Manual dependency injection
builder.Services.AddSingleton<BaseControllerActivator>(
    new ProductControllerActivator(builder.Configuration, loggerFactory));
builder.Services.AddSingleton<BaseControllerActivator>(
    new ProductPricingControllerActivator(builder.Configuration, loggerFactory));
builder.Services.AddSingleton<BaseControllerActivator>(
    new ReviewControllerActivator(builder.Configuration, loggerFactory));
builder.Services.AddSingleton<BaseControllerActivator>(
    new SupplierControllerActivator(builder.Configuration, loggerFactory));
builder.Services.AddSingleton<BaseControllerActivator>(
    new InventoryControllerActivator(builder.Configuration, loggerFactory));

builder.Services.AddSingleton<IControllerActivator>(sp =>
{
    var activators = sp.GetServices<BaseControllerActivator>().ToList();
    return new CompositeControllerActivator(builder.Configuration, activators);
});

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

static void ConfigureMessageQueue<TRequest, TResponse>(IBusRegistrationConfigurator x)
    where TRequest : class
    where TResponse : class
{
    x.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.UseMessageRetry(r =>
        {
            r.Intervals(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
            r.Handle<RabbitMQ.Client.Exceptions.BrokerUnreachableException>();
        });
        cfg.UseScheduledRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15)));

        cfg.Message<TRequest>(e => e.SetEntityName($"{typeof(TRequest).Name.ToLower()}-queue"));
        cfg.Message<TResponse>(e => e.SetEntityName($"{typeof(TResponse).Name.ToLower()}-queue"));
    });
}

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
