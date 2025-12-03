namespace PakManApiBackend.Services;

using System.Text.Json;
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
    public Graph Graph => graph;
    public List<Enemy> Enemies => enemies;


    public GameState(Map map, Graph graph, Player? player, List<Enemy> enemies)
    {
        this.map = map;
        this.graph = graph;
        this.player = player;
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
        if (graph.AdjacencyList.ContainsKey(newEnemy.Position) == false)
        {
            throw new ArgumentException($"Enemy {newEnemy.Name} at {newEnemy.Position} is not valid on the map.");
        }
        enemies.Add(newEnemy);
        enemyCount++;
        Printer.Log("Enemy added at position (" + newEnemy.Position.X + ", " + newEnemy.Position.Y + ")", "info");
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
        if (graph.AdjacencyList.ContainsKey(newPlayer.Position) == false)
        {
            Printer.Log("Invalid position for player.", "warning");
            return false;
        }
        player = newPlayer;
        map.RemoveTile(newPlayer.Position);
        Printer.Log("Player set at position (" + newPlayer.Position.X + ", " + newPlayer.Position.Y + ")", "info");
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
            player.Position = newPos;
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