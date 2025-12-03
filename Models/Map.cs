using PakManApiBackend.Services;
namespace PakManApiBackend.Models;

public class Map
{
    private readonly int columns, rows;
    private int[][] map;
    private int foodCount;
    private int powerUpCount;
    public record Position(int X, int Y);
    public int Columns => columns;
    public int Rows => rows;
    public int FoodCount => foodCount;
    public int PowerUpCount => powerUpCount;
    public int[][] MapArray => map;

    /**
    * Initializes a new instance of the Map class with specified dimensions.
    */
    public Map(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        foodCount = 0;
        powerUpCount = 0;
        map = new int[rows][];
        for (int y = 0; y < rows; y++)
        {
            map[y] = new int[columns];
            for (int x = 0; x < columns; x++)
            {
                SetTile(new Position(x, y), "food", false);
            }
        }
    }

    /**
    * Sets a tile on the map at the specified position.
    */
    public void SetTile(Position pos, string tile, bool removeFirst = true)
    {
        {
            int x = pos.X;
            int y = pos.Y;
            if (removeFirst)
            {
                RemoveTile(new Position(x, y));
            }
            switch (tile)
            {
                case "empty":
                    Printer.Log("Setting empty tile at position (" + x + ", " + y + ")", "info");
                    map[y][x] = 0;
                    break;
                case "wall":
                    Printer.Log("Setting wall at position (" + x + ", " + y + ")", "info");
                    map[y][x] = 1;
                    break;
                case "food":
                    Printer.Log("Setting food at position (" + x + ", " + y + ")", "info");
                    map[y][x] = 2;
                    foodCount++;
                    break;
                case "power-up":
                    Printer.Log("Setting power-up at position (" + x + ", " + y + ")", "info");
                    map[y][x] = 3;
                    powerUpCount++;
                    break;
                default:
                    throw new ArgumentException("Invalid tile type");
            }
        }
    }

    /**
    * Sets a tile on the map for a rectangular area defined by two positions.
    */
    public void SetTile(Position pos1, Position pos2, string tile)
    {

        int x1 = Math.Min(pos1.X, pos2.X);
        int y1 = Math.Min(pos1.Y, pos2.Y);
        int x2 = Math.Max(pos1.X, pos2.X);
        int y2 = Math.Max(pos1.Y, pos2.Y);
        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                SetTile(new Position(x, y), tile);
            }
        }
    }

    /**
    * Gets the type of tile at the specified position.
    */
    public string GetTileType(Position pos)
    {
        int x = pos.X;
        int y = pos.Y;
        int tile = map[y][x];
        return tile switch
        {
            0 => "empty",
            1 => "wall",
            2 => "food",
            3 => "power-up",
            _ => "unknown",
        };
    }

    /**
    * Removes a tile from the map at the specified position.
    */
    public void RemoveTile(Position pos)
    {
        int x = pos.X;
        int y = pos.Y;
        switch (GetTileType(pos))
        {
            case "empty":
                break;
            case "wall":
                Printer.Log("Removing wall at position (" + x + ", " + y + ")", "info");
                break;
            case "food":
                Printer.Log("Removing food at position (" + x + ", " + y + ")", "info");
                foodCount--;
                break;
            case "power-up":
                Printer.Log("Removing power-up at position (" + x + ", " + y + ")", "info");
                powerUpCount--;
                break;
            default:
                throw new ArgumentException("Invalid tile type");
        }
        map[y][x] = 0;
    }

    /**
    * Gets the tile value at the specified position.
    */
    public int GetTile(Position pos)
    {
        int x = pos.X;
        int y = pos.Y;
        return map[y][x];
    }
}