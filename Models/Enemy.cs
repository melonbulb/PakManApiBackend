namespace PakManApiBackend.Models;

public class Enemy : Sprite
{
    protected Enemy() { }
    public Enemy(int id, string name, string img, Map.Position spawnPosition) : base(id, name, img, spawnPosition)
    { }

}