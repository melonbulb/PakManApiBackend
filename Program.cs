
namespace PakManApiBackend;

using Microsoft.EntityFrameworkCore;
using PakManApiBackend.Models;
using PakManApiBackend.Services;

public class AppDbContext : DbContext
{
    // public DbSet<Player> Players { get; set; }
    public DbSet<Score> Scores { get; set; }
    // public DbSet<Enemy> Enemies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=pakman.sqlite;");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add Access-Control-Allow-Origin (CORS) policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://127.0.0.1:3000")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseCors("AllowFrontend");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();
        }

        var gameState = GameGenerator.GenerateCustomMap1();


        app.MapGet("/api/gamestate", () =>
        {
            var response = new
            {
                gameState.Map,
                player = new { gameState.Player, spawnPosition = gameState.Player?.GetSpawnPosition() },
                enemies = gameState.Enemies.Select(e => new { e.Name, spawnPosition = e.GetSpawnPosition() }),
                gameState.Graph
            };

            return Results.Ok(response);
        });

        app.MapPost("/api/score", (Score score) =>
        {
            Console.WriteLine($"Received score from {score.PlayerName} with value {score.Value}");
            using (var db = new AppDbContext())
            {
                db.Add(score);
                db.SaveChanges();
            }
            return Results.Ok();
        });

        app.MapGet("/api/score", () =>
        {
            using (var db = new AppDbContext())
            {
                var scores = db.Scores
                    .OrderByDescending(s => s.Value)
                    .Take(5)
                    .ToList();
                return Results.Ok(scores);
            }
        });

        app.Run();
    }
}
