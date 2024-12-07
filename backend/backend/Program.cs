using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using backend.Database;
using backend.Product.ApplicationLayer;
using backend.Product.InfrastructureLayer;
using backend.Product.InfrastructureLayer.Database;
using backend.Product.ProductControllers;
using backend.Product.ProductPricingControllers;
using backend.Product.Services;
using backend.Shared;
using DbUp;
using DbUp.Engine;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddTransient<TypeParser>();

// Inject Product classes
builder.Services.AddTransient<ProductFieldMaskConfiguration>();
builder.Services.AddScoped<CreateProductExtensions>();
builder.Services.AddAutoMapper(typeof(CreateProductMappingProfile));
builder.Services.AddAutoMapper(typeof(GetProductMappingProfile));
builder.Services.AddAutoMapper(typeof(UpdateProductMappingProfile));

builder.Services.AddTransient<ProductPricingFieldMaskConfiguration>();
builder.Services.AddTransient<GetProductPricingExtensions>();
builder.Services.AddScoped<UpdateProductPricingExtensions>();

// Inject Product Infrastructure
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductViewRepository, ProductViewRepository>();
builder.Services.AddScoped<IProductPricingRepository, ProductPricingRepository>();

// Inject Product Services
builder.Services.AddScoped<IProductApplicationService, ProductApplicationService>();
builder.Services.AddScoped<IProductViewApplicationService, ProductViewApplicationService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.UseOneOfForPolymorphism();
    c.SelectSubTypesUsing(baseType =>
    {
        if (baseType == typeof(GetProductResponse))
        {
            return [typeof(GetPetFoodResponse), typeof(GetGroomingAndHygieneResponse)];
        }

        return [];
    });
});

// Register ApplicationDbContext with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    string? connectionString = configuration.GetConnectionString("DefaultConnection");

    try
    {
        string migrationPath = Path.Combine(Directory.GetCurrentDirectory(), "UpScripts");
        if (connectionString != null)
        {
            ApplyMigrations(connectionString, 5, 2000, migrationPath);
        }

        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations or seeding the database: {ex.Message}");
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
