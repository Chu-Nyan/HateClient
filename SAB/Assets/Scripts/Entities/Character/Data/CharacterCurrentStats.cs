using SAB.Unit;

public class CharacterCurrentStats
{
    public AnimationClipType PlayAnimationClip;
    private float _hp;
    public bool IsMoveing;
    public float MoveSpeed;
    public bool IsAttacking;

    public float HP
    {
        get => _hp;
        set => _hp = value;
    }
}
