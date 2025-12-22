using Core.Interfaces;
using Infrastructure.DataStore;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Service Registered
builder.Services.AddControllers();
// Configure Logging globally
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);
// Register IloggerFactory
// builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
builder.Services.AddDbContext<RepositoryContext>((serviceProvider, options) =>
{
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
            .UseLoggerFactory(loggerFactory);
    // dont use enable sensitave data in production
    if (builder.Environment.IsDevelopment())
    {

        options.EnableSensitiveDataLogging()
             .EnableDetailedErrors();
    }
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Check and apply migrations 
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
    var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<Program>();

    try
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying {count} Pending migrations...", pendingMigrations.Count());
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Migration applied successfully");
        }
        else
        {
            logger.LogInformation("Database is up-to-date. No Migration is pending");
        }
        await SeedStoreContext.SeedAsync(dbContext, loggerFactory);
        logger.LogInformation("Seeding data Completed");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occured while seeding");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
