using UnityEngine;

public struct PatrolData
{
    public static readonly PatrolData Default = new(Vector3.zero, new Vector2(10,-10), new Vector2(10, -10));

    public Vector3 Anchor;
    public Vector2 XRange;
    public Vector2 YRange;

    public PatrolData(Vector3 anchor, Vector2 xRange, Vector2 yRange)
    {
        Anchor = anchor;
        XRange = xRange;
        YRange = yRange;
    }

    public PatrolData(Vector3 anchor)
    {
        Anchor = anchor;
        XRange = new Vector2(10, -10);
        YRange = new Vector2(10, -10);
    }
}
