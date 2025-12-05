namespace PakManApiBackend.Services;

using System.Runtime.CompilerServices;
using PakManApiBackend.Models;

public class Printer
{
    /**
    * Prints a message to the console with a specified type.
    */
    public static void Log(string message, string type = "info", [CallerMemberName] string origin = "")
    {
        switch (type)
        {
            case "info":
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case "warning":
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case "error":
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            default:
                Console.ResetColor();
                break;
        }
        Console.WriteLine(origin + ": " + message);
        Console.ResetColor();
    }

    // /**
    // * Prints the paths from each enemy to the player.
    // */
    // public static void PrintEnemiesToPlayerPaths(GameState game)
    // {
    //     Player? player = game.Player;
    //     List<Enemy> enemies = game.Enemies;
    //     Graph graph = game.Graph;
    //     if (player == null)
    //     {
    //         Log("Player not set. Cannot compute paths.", "error");
    //         return;
    //     }
    //     foreach (var enemy in enemies)
    //     {
    //         var path = graph.GetShortestPath(enemy.GetSpawnPosition(), player.GetSpawnPosition());
    //         if (path != null)
    //         {
    //             Console.WriteLine($"Path from Enemy {enemy.Name} at ({enemy.GetSpawnPosition().X}, {enemy.GetSpawnPosition().Y}) to Player at ({player.GetSpawnPosition().X}, {player.GetSpawnPosition().Y}):");
    //             foreach (var step in path)
    //             {
    //                 Console.Write($"({step.X}, {step.Y}) ");
    //             }
    //             Console.WriteLine();
    //         }
    //         else
    //         {
    //             Console.WriteLine($"No path found from Enemy {enemy.Name} at ({enemy.GetSpawnPosition().X}, {enemy.GetSpawnPosition().Y}) to Player at ({player.GetSpawnPosition().X}, {player.GetSpawnPosition().Y}).");
    //         }
    //     }
    // }

    /**
    * Prints a report of the map details.
    */
    public static void PrintReport(GameState game)
    {
        Map mapOnj = game.Map;
        int columns = mapOnj.Columns;
        int rows = mapOnj.Rows;
        int foodCount = mapOnj.FoodCount;
        int powerUpCount = mapOnj.PowerUpCount;
        int enemyCount = game.EnemyCount;
        Player? player = game.Player;
        List<Enemy> enemies = game.Enemies;
        Console.WriteLine("---------------------------");
        Console.WriteLine("Map Report");
        Console.WriteLine("---------------------------");
        Console.WriteLine($"Dimensions: {columns} columns x {rows} rows");
        Console.WriteLine($"Total Tiles: {columns * rows}");
        Console.WriteLine($"Food Count: {foodCount}");
        Console.WriteLine($"Power-Up Count: {powerUpCount}");
        Console.WriteLine($"Enemy Count: {enemyCount}");
        if (player != null)
        {
            Console.WriteLine($"Player Start GetSpawnPosition(): ({player.GetSpawnPosition().X}, {player.GetSpawnPosition().Y})");
        }
        else
        {
            Console.WriteLine("Player: Not Set");
        }
        if (enemies != null)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Console.WriteLine($"Enemy {enemies[i].Name} Start Position: ({enemies[i].GetSpawnPosition().X}, {enemies[i].GetSpawnPosition().Y})");
            }
        }
        else
        {
            Console.WriteLine("Enemies: Not Set");
        }
    }

    /**
    * Prints the map to the console.
    */
    public static void PrintMap(GameState game, string type = "default")
    {
        Map mapOnj = game.Map;
        int columns = mapOnj.Columns;
        int rows = mapOnj.Rows;
        int[][] map = mapOnj.MapArray;
        Player? player = game.Player;
        List<Enemy> enemies = game.Enemies;

        Console.Write($"{" ",4}");
        for (int x = 0; x < columns; x++)
        {
            Console.Write($"{x,4}");
        }
        Console.WriteLine();
        for (int y = 0; y < rows; y++)
        {
            Console.Write($"{y,4}");
            for (int x = 0; x < columns; x++)
            {
                if (type == "graphical")
                {
                    string message = "";
                    switch (map[y][x])
                    {
                        case 0:
                            message = "";
                            break;
                        case 1:
                            message += "🧱";
                            break;
                        case 2:
                            message += "🍇";
                            break;
                        case 3:
                            message += "💪";
                            break;
                    }
                    if (player != null && player.GetSpawnPosition().X == x && player.GetSpawnPosition().Y == y)
                    {
                        message += "😃";
                    }
                    foreach (var enemy in enemies)
                    {
                        if (enemy.GetSpawnPosition().X == x && enemy.GetSpawnPosition().Y == y)
                        {
                            message += "👻";
                        }
                    }
                    Console.Write($"{message,4}");
                }
                else
                {
                    Console.Write($"{map[y][x],4}");
                }
            }
            Console.WriteLine();
        }
    }
}