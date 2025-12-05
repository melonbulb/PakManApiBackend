namespace PakManApiBackend.Services;

using PakManApiBackend.Models;

/**
 * Builds a graph from the map for pathfinding.
 */
public static class GraphBuilder
{
    public static Graph BuildGraph(Map map)
    {
        int rows = map.Rows;
        int columns = map.Columns;
        Graph graph = new Graph();
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Map.Position currentPos = new Map.Position(x, y);
                if (map.GetTileType(currentPos) != "wall")
                {
                    // Check all four directions
                    Map.Position[] directions =
                        {
                            new Map.Position(x, y - 1), // Up
                            new Map.Position(x, y + 1), // Down
                            new Map.Position(x - 1, y), // Left
                            new Map.Position(x + 1, y)  // Right
                        };

                    foreach (var dir in directions)
                    {
                        if (dir.X >= 0 && dir.X < columns && dir.Y >= 0 && dir.Y < rows)
                        {
                            if (map.GetTileType(dir) != "wall")
                            {
                                graph.AddEdge(currentPos, dir);
                            }
                        }
                    }
                }
            }
        }
        return graph;
    }
}