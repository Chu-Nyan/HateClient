using UnityEngine;

public struct PatrolData
{
    public Vector3 RespawnPosition;
    public Vector2 XRange;
    public Vector2 YRange;

    public PatrolData(Vector3 respawn, Vector2 xRange, Vector2 yRange)
    {
        RespawnPosition = respawn;
        XRange = xRange;
        YRange = yRange;
    }
}
