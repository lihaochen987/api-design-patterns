using backend.Database;
using backend.Product.Database;
using backend.Product.FieldMasks;
using backend.Product.ProductControllers;
using backend.Product.ProductPricingControllers;
using backend.Product.Services;
using backend.Shared;
using DbUp;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddTransient<GetProductExtensions>();
builder.Services.AddScoped<ReplaceProductExtensions>();
builder.Services.AddScoped<UpdateProductExtensions>();

builder.Services.AddTransient<ProductPricingFieldMaskConfiguration>();
builder.Services.AddTransient<GetProductPricingExtensions>();
builder.Services.AddScoped<UpdateProductPricingExtensions>();

// Inject Product services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductViewRepository, ProductViewRepository>();
builder.Services.AddScoped<IProductPricingRepository, ProductPricingRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });

// Register ApplicationDbContext with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    try
    {
        var migrationPath = Path.Combine(Directory.GetCurrentDirectory(), "UpScripts");
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

    for (var attempt = 1; attempt <= retryCount; attempt++)
    {
        try
        {
            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsFromFileSystem(migrationPath)
                .LogToConsole()
                .WithTransaction()
                .Build();

            var result = upgrader.PerformUpgrade();
            if (!result.Successful) throw new Exception("Migration failed: " + result.Error);
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