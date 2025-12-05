namespace PakManApiBackend.Models;

public class Player : Sprite
{
    public int HighestScore { get; set; }

    protected Player() { }

    public Player(int id, string name, string img, Map.Position spawnPosition) : base(id, name, img, spawnPosition)
    { }

    public void SetHighestScore(int score, int lives)
    {
        HighestScore = score + (lives - 1) * 100;
        new Score(Name, HighestScore);
    }
}