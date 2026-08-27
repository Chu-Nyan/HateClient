using Chu.Data;

public class EngineSetting
{
    public static readonly EngineSetting Default = new("en", new(0, 100, 0, 100), 4);

    public string Language;
    public bool UseCollision = true;
    public readonly RectBound CollisionBound;
    public int Capacity;

    public EngineSetting(string language, RectBound collisionBound, int capacity)
    {
        Language = language;
        UseCollision = true;
        CollisionBound = collisionBound;
        Capacity = capacity;
    }
}
