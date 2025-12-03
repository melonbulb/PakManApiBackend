
namespace PakManApiBackend;

using PakManApiBackend.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var gameState = GameGenerator.GenerateCustomMap1();

        app.MapGet("/gamestate", () =>
        {
            var response = new
            {
                gameState.Map,
                gameState.Player,
                gameState.Enemies
            };

            return Results.Ok(response);
        });



        app.Run();
    }
}
