using Chu.Utility.UnityHelper;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Chu.Collision
{
    [Serializable]
    public struct ShapeParam
    {
        public ShapeType Type;
        public float OffsetX;
        public float OffsetY;
        public float Param1;
        public float Param2;

        [JsonIgnore]
        public readonly float Radius
        {
            get => Param1;
        }

        [JsonIgnore]
        public readonly float Width
        {
            get => Param1;
        }

        [JsonIgnore]
        public readonly float Height
        {
            get => Param2;
        }

        public ShapeParam(ShapeType type, float offsetX, float offsetY, float param1, float param2)
        {
            if (type == ShapeType.Composite)
                throw new Exception();

            Type = type;
            OffsetX = offsetX;
            OffsetY = offsetY;
            Param1 = param1;
            Param2 = param2;
        }

        public readonly RectRangeData GetRectData()
        {
            if (Type != ShapeType.Rectangle)
                Debug.LogWarning("지정된 타입과 호환되지 않는 데이터를 사용 중");

            return new RectRangeData(new Vector2(OffsetX, OffsetY), 0, Width, Height);
        }

        public readonly CircleRangeData GetCircleData()
        {
            if (Type != ShapeType.Circle)
                Debug.LogWarning("지정된 타입과 호환되지 않는 데이터를 사용 중");

            return new CircleRangeData(new Vector2(OffsetX, OffsetY), Radius);
        }

        public readonly void DrawGizmo(Vector3 anchor, Quaternion quaternion)
        {
            Vector3 center = new(anchor.x + OffsetX, anchor.y, anchor.z + OffsetY);

            Gizmos.color = Color.yellow;
            if (Type == ShapeType.Rectangle)
            {
                GizmoDrawer.DrawRectangle(center, Width, Height, quaternion);
            }
            else if (Type == ShapeType.Circle)
            {
                GizmoDrawer.DrawCircle(center, Radius);
            }
        }

        public static IShape GetShape(ShapeParam[] data)
        {
            if (data.Length > 1)
            {
                var shape = ShapeFactory.Instance.Generate<CompositeShape>();
                shape.Setup(data);
                return shape;
            }
            else if (data[0].Type == ShapeType.Rectangle)
            {
                var shape = ShapeFactory.Instance.Generate<RectShape>();
                shape.Setup(data[0].GetRectData());
                return shape;

            }
            else if (data[0].Type == ShapeType.Circle)
            {
                var shape = ShapeFactory.Instance.Generate<CircleShape>();
                shape.Setup(data[0].GetCircleData());
                return shape;
            }

            throw new Exception();
        }
    }
}
