public struct IdleData
{
    public static readonly IdleData Default = new(3);

    public float WaitTime;

    public IdleData(float time)
    {
        WaitTime = time;
    }
}
