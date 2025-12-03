namespace PakManApiBackend.Models;

/**
 * Represents a graph structure for pathfinding on the map.
 */
public class Graph
{
    private Dictionary<Map.Position, List<Map.Position>> adjacencyList;
    public Dictionary<Map.Position, List<Map.Position>> AdjacencyList => adjacencyList;

    /**
    * Initializes a new instance of the Graph class.
    */
    public Graph()
    {
        adjacencyList = new Dictionary<Map.Position, List<Map.Position>>();
    }

    /**
    * Adds a directed edge from one position to another in the graph.
    */
    public void AddEdge(Map.Position from, Map.Position to)
    {
        if (!adjacencyList.ContainsKey(from))
        {
            adjacencyList[from] = new List<Map.Position>();
        }
        adjacencyList[from].Add(to);
    }

    /**
    * Gets the neighbors of a given position in the graph.
    */
    public List<Map.Position> GetNeighbors(Map.Position position)
    {
        if (adjacencyList.ContainsKey(position))
        {
            return adjacencyList[position];
        }
        return new List<Map.Position>();
    }

    /**
    * Checks if there is a directed edge from one position to another in the graph.
    */
    public bool HasEdge(Map.Position from, Map.Position to)
    {
        return adjacencyList.ContainsKey(from) && adjacencyList[from].Contains(to);
    }

    /**
    * Prints the graph to the console.
    */
    public void PrintGraph()
    {
        foreach (var kvp in adjacencyList)
        {
            Console.Write(kvp.Key.ToString() + " -> ");
            Console.WriteLine(string.Join(", ", kvp.Value.Select(pos => pos.ToString())));
        }
    }

    /**
    * Finds the shortest path between two positions using BFS.
    */
    public List<Map.Position> GetShortestPath(Map.Position from, Map.Position to)
    {
        var queue = new Queue<Map.Position>();
        var visited = new HashSet<Map.Position> { from };
        var predecessors = new Dictionary<Map.Position, Map.Position>();

        queue.Enqueue(from);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.Equals(to))
                break;

            foreach (var neighbor in adjacencyList[current])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    // Set the predecessor of the neighbor to the current node
                    predecessors[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        // Reconstruct path
        var path = new List<Map.Position>();
        if (!visited.Contains(to))
        {
            // No path found
            return path;
        }

        // Build the path by backtracking from end to start
        var at = to;
        while (at != null)
        {
            path.Add(at);
            if (at.Equals(from))
                break;
            at = predecessors[at];
        }
        // Reverse the path to get it from start to end
        path.Reverse();

        return path;
    }

    /**
    * Prints the shortest path between two positions to the console.
    */
    public void PrintShortestPath(Map.Position from, Map.Position to)
    {
        var path = GetShortestPath(from, to);
        if (path.Count == 0)
        {
            Console.WriteLine("No path found from " + from.ToString() + " to " + to.ToString());
        }
        else
        {
            Console.WriteLine("Shortest path from " + from.ToString() + " to " + to.ToString() + ":");
            Console.WriteLine(string.Join(" -> ", path.Select(pos => pos.ToString())));
        }
    }
}