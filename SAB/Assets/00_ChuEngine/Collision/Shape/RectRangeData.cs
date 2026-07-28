using UnityEngine;

public struct RectRangeData
{
    public static RectRangeData Default = new RectRangeData(Vector3.zero, 0, 1, 1);

    public Vector3 Offset;
    public float Rotation;
    public float Width;
    public float Height;

    public RectRangeData(Vector3 offset, float rotation, float width, float height)
    {
        Offset = offset;
        Rotation = rotation;
        Width = width;
        Height = height;
    }
}
