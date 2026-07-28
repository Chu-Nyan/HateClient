using SAB.Unit;

public interface IHasStats
{
    public float this[StatType type] { get; }
    public void AddHP(float value);
}
