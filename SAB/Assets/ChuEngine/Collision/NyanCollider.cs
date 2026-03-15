using Chu.Collision.Layer;
using Chu.Data;
using System;
using System.Collections.Generic;

namespace Chu.Collision
{
    /// <summary>
    /// NyanCollisonSystem에서 사용하는 물리 오브젝트
    /// </summary>
    public class NyanCollider : IQuadTreeEntity
    {
        public readonly int InstanceID;
        private readonly INyanCollisionProvider _provider;
        private readonly HashSet<int> _insertedNodes;
        private readonly HashSet<int> _contactIDs;
        private int _instigatorID;
        private IShape _shape;

        private NyanLayer _layer;
        private NyanLayerMask _mask;

        private bool _isActive;
        private string _comment;

        private event Action<NyanCollider> PositionChanged;
        private event Action<NyanCollider> EnabledChanged;

        public INyanCollisionProvider Provider
        {
            get => _provider;
        }

        public int InstigatorID
        {
            get => _instigatorID;
        }

        public HashSet<int> InsertedNodesID
        {
            get => _insertedNodes;
        }

        public HashSet<int> ContactIDs
        {
            get => _contactIDs;
        }

        public ShapeType ShapeType
        {
            get => _shape.ShapeType;
        }

        public RectBound RectBound
        {
            get => _shape.AABB;
        }

        public NyanLayer Layer
        {
            get => _layer;
        }

        public NyanLayerMask LayerMask
        {
            get => _mask;
        }

        public bool IsActive
        {
            get => _isActive;
        }

        public string Comment
        {
            get => _comment;
        }

        public NyanCollider(INyanCollisionProvider provider, int id, string comment = null)
        {
            InstanceID = id;
            _provider = provider;
            _insertedNodes = new(4);
            _contactIDs = new();
            _comment = comment;
        }

        public void SetLayer(NyanLayer layer, NyanLayerMask mask)
        {
            _layer = layer;
            _mask = mask;
        }

        public void SetInstigatorID(int id)
        {
            _instigatorID = id;
        }

        public void SetShape(IShape shape)
        {
            if (_shape != null)
                ShapeFactory.Instance.Release(_shape);
            if (shape == null)
                SetActive(false);

            _shape = shape;
        }

        public void SetActive(bool value)
        {
            if (_isActive == value)
                return;
            if (_shape == null)
                value = false;

            _isActive = value;
            EnabledChanged?.Invoke(this);
        }

        public void RefreshTransform()
        {
            PositionChanged?.Invoke(this);
        }

        public void UpdateAABB()
        {
            _shape.UpdateAABB(_provider.transform.position, _provider.transform.eulerAngles.y);
        }

        public bool Intersects(IShape shape)
        {
            return _shape.Intersects(shape);
        }

        public bool Intersects(NyanCollider collider)
        {
            bool result = false;
            if (RectBound.IsIntersecting(RectBound, collider.RectBound) == true)
                result = _shape.Intersects(collider._shape);

            return result;
        }

        public bool IsContacted(int id)
        {
            return _contactIDs.Contains(id);
        }

        public void RegisterQuadTreeNodeID(int number)
        {
            _insertedNodes.Add(number);
        }

        public void ResetInsertedNodes()
        {
            _insertedNodes.Clear();
        }

        public void AddContactColliderID(int id)
        {
            _contactIDs.Add(id);
        }

        public void RemoveContactColliderID(int id)
        {
            _contactIDs.Remove(id);
        }


        #region 델리게이트 등록, 해제
        public void RegisterPositionChanged(Action<NyanCollider> action)
        {
            PositionChanged += action;
        }

        public void UnregisterPositionChanged(Action<NyanCollider> action)
        {
            PositionChanged -= action;
        }

        public void RegisterEnabled(Action<NyanCollider> action)
        {
            EnabledChanged += action;
        }
        #endregion

        public override string ToString()
        {
            return $"{InstanceID} {_comment}";
        }

#if UNITY_EDITOR
        public bool IsValid(out string log)
        {
            var sb = new System.Text.StringBuilder();

            if (_provider == null)
                sb.AppendLine("Provider is null");

            log = sb.ToString();
            return sb.Length == 0;
        }
#endif
    }
}
