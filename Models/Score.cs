namespace PakManApiBackend.Models;

public class Score
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public int Value { get; set; }
    public DateTime AchievedAt { get; set; }

    // For EF Core
    protected Score() { }

    public Score(string playerName, int value)
    {
        PlayerName = playerName;
        Value = value;
        AchievedAt = DateTime.UtcNow;
    }
}