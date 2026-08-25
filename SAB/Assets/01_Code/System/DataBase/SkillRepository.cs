using Chu.Collision;
using Chu.Core;
using SAB.Skill;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.DataManger
{
    public class SkillRepository
    {
        private const string BasePath = "SkillBaseData";

        private const string SequencePath = "SkillSequenceData";
        private const string HitBoxesPath = "SkillHitBoxData";
        private const string CollisionLogicPath = "SkillCollisionLogicData";

        private const string InstancePath = "SkillStepInstanceData";
        private const string AoePath = "SkillStepAoEData";
        private const string DotPath = "SkillStepDoTData";
        private const string TimerPath = "SkillStepTimerData";

        private const string SkillSetPath = "SkillSetData";

        public readonly Dictionary<int, SkillData> SkillByID;
        public readonly Dictionary<int, int[]> SkillSet;
        public readonly Dictionary<int, IStepData[]> SequenceByID;
        public readonly Dictionary<int, ShapeParam[]> HitBoxByID;
        public readonly Dictionary<int, CollisionLogicData[]> CollisionLogicByID;

        public readonly Dictionary<int, InstanceStepData> InstanceStepByID;
        public readonly Dictionary<int, AoEStepData> AoEStepByID;
        public readonly Dictionary<int, DotStepData> DotStepByID;
        public readonly Dictionary<int, TimerStepData> TimerStepByID;

        public SkillRepository()
        {
            InstanceStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepInstanceDto>(AssetManager.LoadJson(InstancePath)),
                keySelector: dto => dto.ID,
                converter: dto => new InstanceStepData(dto.ID, dto.DamageRate, dto.Count)
            );

            AoEStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepAoEDto>(AssetManager.LoadJson(AoePath)),
                keySelector: dto => dto.ID,
                converter: dto => new AoEStepData(dto.ID, dto.DamageRate, dto.Ranged, dto.Count)
            );

            DotStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepDoTDto>(AssetManager.LoadJson(DotPath)),
                keySelector: dto => dto.ID,
                converter: dto => new DotStepData(dto.ID, dto.DamageRate, dto.Duration)
            );

            TimerStepByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillStepTimerDto>(AssetManager.LoadJson(TimerPath)),
                keySelector: dto => dto.ID,
                converter: dto => new TimerStepData(dto.ID, dto.Duration)
            );

            SequenceByID = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillFlowStepDto>(AssetManager.LoadJson(SequencePath)),
                keySelector: dto => dto.ID,
                converter: dto => GetStepData(dto.LogicID)
            );

            HitBoxByID = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillHitBoxDto>(AssetManager.LoadJson(HitBoxesPath)),
                keySelector: dto => dto.ID,
                converter: dto => new ShapeParam(dto.ShapeType, dto.OffsetX, dto.OffsetY, dto.Param1, dto.Param2)
            );

            CollisionLogicByID = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillCollisionLogicDto>(AssetManager.LoadJson(CollisionLogicPath)),
                keySelector: dto => dto.ID,
                converter: dto =>
                {
                    var shape = ShapeParam.ConvertShape(HitBoxByID[dto.HitboxID]);
                    shape.UpdateAABB(Vector2.zero, 0);
                    var mesh = CreateMesh(shape);
                    return new CollisionLogicData(dto.ID, dto.Order, dto.ActiveTime, dto.Speed, HitBoxByID[dto.HitboxID], SequenceByID[dto.SequenceID], mesh);
                }
            );

            SkillByID = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<SkillBaseDto>(AssetManager.LoadJson(BasePath)),
                keySelector: dto => dto.ID,
                converter: dto => new SkillData(dto, CollisionLogicByID[dto.ObjectLogicID])
            );

            SkillSet = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<SkillSetDto>(AssetManager.LoadJson(SkillSetPath)),
                keySelector: dto => dto.SetID,
                converter: dto => dto.SkillID
            );
        }

        private IStepData GetStepData(int id)
        {
            if (InstanceStepByID.ContainsKey(id) == true)
                return InstanceStepByID[id];
            if (AoEStepByID.ContainsKey(id) == true)
                return AoEStepByID[id];
            if (DotStepByID.ContainsKey(id) == true)
                return DotStepByID[id];
            if (TimerStepByID.ContainsKey(id) == true)
                return TimerStepByID[id];

            throw new Exception($"Don't have skill step {id}");
        }

        private Mesh CreateMesh(IShape shape)
        {
            if (shape.ShapeType == ShapeType.Rectangle)
                return CreateMesh((RectShape)shape);
            else if (shape.ShapeType == ShapeType.Circle)
                return CreateMesh((CircleShape)shape);
            else if (shape.ShapeType == ShapeType.Composite)
                return CreateMesh((CompositeShape)shape);

            throw new Exception();
        }

        private Mesh CreateMesh(CompositeShape shape)
        {
            CombineInstance[] combines = new CombineInstance[shape.Count];
            for (int i = 0; i < shape.Count; i++)
            {
                combines[i] = new CombineInstance
                {
                    mesh = CreateMesh(shape[i]),
                    transform = Matrix4x4.identity
                };
            }

            Mesh mesh = new();

            mesh.CombineMeshes(combines, true, false);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        private Mesh CreateMesh(RectShape shape)
        {
            Mesh mesh = new();

            Vector3 right = new(shape.Radius[0].x, 0f, shape.Radius[0].y);
            Vector3 up = new(shape.Radius[1].x, 0f, shape.Radius[1].y);
            Vector3 offset = new(shape.Data.Offset.x, 0f, shape.Data.Offset.y);
            Vector3[] vertices = { -right - up + offset, -right + up + offset, right + up + offset, right - up + offset };
            int[] triangles = { 0, 1, 2, 0, 2, 3 };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        private Mesh CreateMesh(CircleShape shape, int segments = 32)
        {
            Mesh mesh = new();

            Vector3[] vertices = new Vector3[segments + 1];
            int[] triangles = new int[segments * 3];

            Vector3 offset = new(shape.Data.Offset.x, 0f, shape.Data.Offset.y);
            float angleStep = Mathf.PI * 2f / segments;
            vertices[0] = offset;

            for (int i = 0; i < segments; i++)
            {
                float angle = angleStep * i;
                float rad = shape.Data.Radius;
                vertices[i + 1] = offset + new Vector3(Mathf.Cos(angle) * rad, 0f, Mathf.Sin(angle) * rad);
            }

            for (int i = 0; i < segments; i++)
            {
                int current = i + 1;
                int next = (i + 1) % segments + 1;

                int index = i * 3;

                triangles[index] = 0;
                triangles[index + 1] = next;
                triangles[index + 2] = current;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
