namespace PakManApiBackend.Tests;

using Xunit;

using PakManApiBackend.Models;
using PakManApiBackend.Services;

public class GameStateTests
{
    [Fact]
    public void TestSetPlayer_ValidPosition_SetsPlayer()
    {
        // Arrange
        var gameState = GameGenerator.GenerateCustomMap1();
        var player = new Player(1, "TestPlayer", "test.png", new Map.Position(5, 1));

        // Act
        bool result = gameState.SetPlayer(player);

        // Assert
        Assert.True(result);
        Assert.Equal(player, gameState.Player);
    }
}