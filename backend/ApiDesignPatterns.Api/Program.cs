using System.Data;
using System.Text.Json.Serialization;
using backend;
using backend.Inventory;
using backend.Inventory.InfrastructureLayer.Mappers;
using backend.Product;
using backend.Product.InfrastructureLayer.Mappers;
using backend.Review;
using backend.Shared;
using backend.Shared.ControllerActivators;
using backend.Supplier;
using Dapper;
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

// Product
SqlMapper.AddTypeHandler(new NameTypeHandler());
SqlMapper.AddTypeHandler(new IngredientsTypeHandler());
SqlMapper.AddTypeHandler(new StorageInstructionsTypeHandler());
SqlMapper.AddTypeHandler(new WeightTypeHandler());
SqlMapper.AddTypeHandler(new UsageInstructionsTypeHandler());
SqlMapper.AddTypeHandler(new SafetyWarningsTypeHandler());

// Review
SqlMapper.AddTypeHandler(new QuantityTypeHandler());

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    string? connectionString = configuration.GetConnectionString("DefaultConnection");

    try
    {
        if (connectionString != null)
        {
            string migrationPath = Path.Combine(Directory.GetCurrentDirectory(), "UpScripts");
            DatabaseMigrations.Apply(connectionString, 5, 2000, migrationPath);
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
