namespace PakManApiBackend.Services;

using PakManApiBackend.Models;

public class GameState
{
    Map map;
    Player? player;
    Graph graph;
    List<Enemy> enemies;

    private int enemyCount;
    public int EnemyCount => enemyCount;
    public int ConsumablesCount => map.PowerUpCount + map.FoodCount;

    public Map Map => map;
    public Player? Player => player;
    public Dictionary<string, string[]> Graph => graph?.AdjacencyList?.ToDictionary(
        kvp => $"{kvp.Key.X},{kvp.Key.Y}",
        kvp => kvp.Value.Select(pos => $"{pos.X},{pos.Y}").ToArray()
    ) ?? new Dictionary<string, string[]>();
    public List<Enemy> Enemies => enemies;


    public GameState(Map map, Graph graph, Player? player, List<Enemy> enemies)
    {
        this.map = map;
        this.graph = graph;
        if (player != null)
        {
            SetPlayer(player);
        }
        else
        {
            this.player = null;
        }
        enemyCount = 0;
        this.enemies = new List<Enemy>();
        foreach (var enemy in enemies)
        {
            AddEnemy(enemy);
        }

    }

    /**
    * Adds an enemy to the map.
    */
    public bool AddEnemy(Enemy newEnemy)
    {
        if (graph == null)
        {
            throw new InvalidOperationException("Graph not generated. Call GenerateGraph() before setting the player.");
        }
        if (graph.AdjacencyList.ContainsKey(newEnemy.GetSpawnPosition()) == false)
        {
            throw new ArgumentException($"Enemy {newEnemy.Name} at {newEnemy.GetSpawnPosition()} is not valid on the map.");
        }
        enemies.Add(newEnemy);
        enemyCount++;
        Printer.Log("Enemy added at position (" + newEnemy.GetSpawnPosition().X + ", " + newEnemy.GetSpawnPosition().Y + ")", "info");
        return true;
    }

    /**
    * Removes an enemy from the map at the specified index.
    */
    public bool RemoveEnemyAt(int index)
    {
        if (index >= 0 && index < enemies.Count)
        {
            enemies.RemoveAt(index);
            enemyCount--;
            return true;
        }
        return false;
    }

    /**
    * Sets the player on the map.
    */
    public bool SetPlayer(Player newPlayer)
    {
        if (graph == null)
        {
            throw new InvalidOperationException("Graph not generated. Call GenerateGraph() before setting the player.");
        }
        if (graph.AdjacencyList.ContainsKey(newPlayer.GetSpawnPosition()) == false)
        {
            Printer.Log("Invalid position for player.", "warning");
            return false;
        }
        player = newPlayer;
        map.RemoveTile(newPlayer.GetSpawnPosition());
        Printer.Log("Player set at position (" + newPlayer.GetSpawnPosition().X + ", " + newPlayer.GetSpawnPosition().Y + ")", "info");
        return true;
    }

    /**
    * Updates the player's position on the map.
    */
    public bool UpdatePlayerPosition(Map.Position newPos)
    {
        if (graph == null)
        {
            throw new InvalidOperationException("Graph not generated. Call GenerateGraph() before setting the player.");
        }
        if (graph.AdjacencyList.ContainsKey(newPos) == false)
        {
            return false;
        }
        if (player != null)
        {
            player.SetSpawnPosition(newPos);
            map.RemoveTile(newPos);
            return true;
        }
        return false;
    }

    /**
    * Removes the player from the map.
    */
    public bool RemovePlayer()
    {
        if (player != null)
        {
            player = null;
            return true;
        }
        return false;
    }
}