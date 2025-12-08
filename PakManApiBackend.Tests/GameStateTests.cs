namespace PakManApiBackend.Tests;

using Xunit;

using PakManApiBackend.Models;
using PakManApiBackend.Services;

public class GameStateTests
{
    [Fact]
    public void TestAddEnemy_ValidEnemy_AddsSuccessfully()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var player = new Player(1, "TestPlayer", "sample.png", new Map.Position(1, 1));
        var enemies = new List<Enemy>();

        // Create a valid enemy position
        var validEnemyPosition = new Map.Position(2, 2);
        graph.AdjacencyList[validEnemyPosition] = new List<Map.Position> { new Map.Position(1, 2), new Map.Position(2, 1) };

        var gameState = new GameState(map, graph, player, enemies);
        var enemy = new Enemy(1, "Ghost", "ghost.png", validEnemyPosition);

        // Act
        var result = gameState.AddEnemy(enemy);

        // Assert
        Assert.True(result);
        Assert.Contains(enemy, gameState.Enemies);
    }

    [Fact]
    public void TestAddEnemy_InvalidPosition_ThrowsException()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var validPosition = new Map.Position(1, 1);
        graph.AdjacencyList[validPosition] = new List<Map.Position>();

        var gameState = new GameState(map, graph, null, new List<Enemy>());
        var invalidPosition = new Map.Position(99, 99);
        var enemy = new Enemy(1, "Ghost", "ghost.png", invalidPosition);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => gameState.AddEnemy(enemy));
    }

    [Fact]
    public void TestRemoveEnemyAt_ValidIndex_RemovesSuccessfully()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var validPosition = new Map.Position(2, 2);
        graph.AdjacencyList[validPosition] = new List<Map.Position>();

        var enemies = new List<Enemy>();
        var enemy1 = new Enemy(1, "Ghost1", "ghost.png", validPosition);
        var enemy2 = new Enemy(2, "Ghost2", "ghost.png", validPosition);
        enemies.Add(enemy1);
        enemies.Add(enemy2);

        var gameState = new GameState(map, graph, null, enemies);

        // Act
        var result = gameState.RemoveEnemyAt(0);

        // Assert
        Assert.True(result);
        Assert.Equal(1, gameState.EnemyCount);
        Assert.DoesNotContain(enemy1, gameState.Enemies);
    }

    [Fact]
    public void TestRemoveEnemyAt_InvalidIndex_ReturnsFalse()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var gameState = new GameState(map, graph, null, new List<Enemy>());

        // Act
        var result = gameState.RemoveEnemyAt(5);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestSetPlayer_ValidPosition_SetsSuccessfully()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var validPosition = new Map.Position(3, 3);
        graph.AdjacencyList[validPosition] = new List<Map.Position>();

        var gameState = new GameState(map, graph, null, new List<Enemy>());
        var player = new Player(1, "TestPlayer", "player.png", validPosition);

        // Act
        var result = gameState.SetPlayer(player);

        // Assert
        Assert.True(result);
        Assert.NotNull(gameState.Player);
        Assert.Equal("TestPlayer", gameState.Player.Name);
    }

    [Fact]
    public void TestSetPlayer_InvalidPosition_ReturnsFalse()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var validPosition = new Map.Position(1, 1);
        graph.AdjacencyList[validPosition] = new List<Map.Position>();

        var gameState = new GameState(map, graph, null, new List<Enemy>());
        var invalidPosition = new Map.Position(99, 99);
        var player = new Player(1, "TestPlayer", "player.png", invalidPosition);

        // Act
        var result = gameState.SetPlayer(player);

        // Assert
        Assert.False(result);
        Assert.Null(gameState.Player);
    }

    [Fact]
    public void TestUpdatePlayerPosition_ValidPosition_UpdatesSuccessfully()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var startPosition = new Map.Position(1, 1);
        var newPosition = new Map.Position(2, 2);
        graph.AdjacencyList[startPosition] = new List<Map.Position>();
        graph.AdjacencyList[newPosition] = new List<Map.Position>();

        var player = new Player(1, "TestPlayer", "player.png", startPosition);
        var gameState = new GameState(map, graph, player, new List<Enemy>());

        // Act
        var result = gameState.UpdatePlayerPosition(newPosition);

        // Assert
        Assert.True(result);
        Assert.Equal(newPosition.X, gameState.Player!.GetSpawnPosition().X);
        Assert.Equal(newPosition.Y, gameState.Player.GetSpawnPosition().Y);
    }

    [Fact]
    public void TestUpdatePlayerPosition_InvalidPosition_ReturnsFalse()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var startPosition = new Map.Position(1, 1);
        graph.AdjacencyList[startPosition] = new List<Map.Position>();

        var player = new Player(1, "TestPlayer", "player.png", startPosition);
        var gameState = new GameState(map, graph, player, new List<Enemy>());
        var invalidPosition = new Map.Position(99, 99);

        // Act
        var result = gameState.UpdatePlayerPosition(invalidPosition);

        // Assert
        Assert.False(result);
        Assert.Equal(startPosition.X, gameState.Player!.GetSpawnPosition().X);
    }

    [Fact]
    public void TestUpdatePlayerPosition_NoPlayer_ReturnsFalse()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var position = new Map.Position(1, 1);
        graph.AdjacencyList[position] = new List<Map.Position>();

        var gameState = new GameState(map, graph, null, new List<Enemy>());

        // Act
        var result = gameState.UpdatePlayerPosition(position);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestRemovePlayer_PlayerExists_RemovesSuccessfully()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var position = new Map.Position(1, 1);
        graph.AdjacencyList[position] = new List<Map.Position>();

        var player = new Player(1, "TestPlayer", "player.png", position);
        var gameState = new GameState(map, graph, player, new List<Enemy>());

        // Act
        var result = gameState.RemovePlayer();

        // Assert
        Assert.True(result);
        Assert.Null(gameState.Player);
    }

    [Fact]
    public void TestRemovePlayer_NoPlayer_ReturnsFalse()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var gameState = new GameState(map, graph, null, new List<Enemy>());

        // Act
        var result = gameState.RemovePlayer();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestEnemyCount_MultipleEnemies_ReturnsCorrectCount()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var position1 = new Map.Position(1, 1);
        var position2 = new Map.Position(2, 2);
        var position3 = new Map.Position(3, 3);
        graph.AdjacencyList[position1] = new List<Map.Position>();
        graph.AdjacencyList[position2] = new List<Map.Position>();
        graph.AdjacencyList[position3] = new List<Map.Position>();

        var enemies = new List<Enemy>
        {
            new Enemy(1, "Ghost1", "ghost.png", position1),
            new Enemy(2, "Ghost2", "ghost.png", position2),
            new Enemy(3, "Ghost3", "ghost.png", position3)
        };

        var gameState = new GameState(map, graph, null, enemies);

        // Assert
        Assert.Equal(3, gameState.EnemyCount);
    }

    [Fact]
    public void TestGraph_Property_ReturnsSerializedDictionary()
    {
        // Arrange
        var map = new Map(5, 5);
        var graph = new Graph();
        var position = new Map.Position(1, 1);
        var adjacentPosition = new Map.Position(2, 1);
        graph.AdjacencyList[position] = new List<Map.Position> { adjacentPosition };

        var gameState = new GameState(map, graph, null, new List<Enemy>());

        // Act
        var graphDict = gameState.Graph;

        // Assert
        Assert.NotNull(graphDict);
        Assert.Contains("1,1", graphDict.Keys);
        Assert.Contains("2,1", graphDict["1,1"]);
    }
}