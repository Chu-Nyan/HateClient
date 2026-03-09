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
        private float _param1;
        private float _param2;

        public float Radius
        {
            get => _param1;
        }

        public float Width
        {
            get => _param1;
        }

        public float Height
        {
            get => _param2;
        }

        public ShapeParam(ShapeType type, float offsetX, float offsetY, float param1, float param2)
        {
            if (type == ShapeType.Composite)
                throw new Exception();

            Type = type;
            OffsetX = offsetX;
            OffsetY = offsetY;
            _param1 = param1;
            _param2 = param2;
        }

        public RectRangeData GetRectData()
        {
            if (Type != ShapeType.Rectangle)
                Debug.LogWarning("지정된 타입과 호환되지 않는 데이터를 사용 중");

            return new RectRangeData(new Vector2(OffsetX, OffsetY), 0, Width, Height);
        }

        public CircleRangeData GetCircleData()
        {
            if (Type != ShapeType.Rectangle)
                Debug.LogWarning("지정된 타입과 호환되지 않는 데이터를 사용 중");

            return new CircleRangeData(new Vector2(OffsetX, OffsetY), Radius);
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