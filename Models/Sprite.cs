namespace PakManApiBackend.Models;

public class Sprite
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    private Map.Position spawnPosition { get; set; }

    // For EF Core
    protected Sprite() { }

    public Sprite(int id, string name, string imagePath, Map.Position spawnPosition)
    {
        Id = id;
        Name = name;
        ImagePath = imagePath;
        this.spawnPosition = spawnPosition;
    }

    public Map.Position GetSpawnPosition()
    {
        return spawnPosition;
    }

    public void SetSpawnPosition(Map.Position newPosition)
    {
        spawnPosition = newPosition;
    }
}