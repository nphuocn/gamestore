using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("GameStoreSqlite");
        builder.Services.AddSqlite<GameStoreContext>(
            connectionString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Name = "Action" },
                        new Genre { Name = "Adventure" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Strategy" },
                        new Genre { Name = "Action-Adventure" },
                        new Genre { Name = "Simulation" }
                    );

                    context.SaveChanges();
                }
            })
        );
    }

    public static void AddOriginCORS(this WebApplicationBuilder builder)
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowOrigins").Get<string[]>();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
    }
}