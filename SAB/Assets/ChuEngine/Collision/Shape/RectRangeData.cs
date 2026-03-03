using UnityEngine;

public struct RectRangeData
{
    public Vector3 Offset;
    public float Rotation;
    public float Width;
    public float Height;

    public RectRangeData(Vector3 offset, float radian, float width, float height)
    {
        Offset = offset;
        Rotation = radian;
        Width = width;
        Height = height;
    }
}
