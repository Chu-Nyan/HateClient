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
        private Shape _shape;

        private NyanLayer _layer;
        private NyanLayerMask _mask;
        private bool _isLayerInitialized;

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

        public Shape Shape
        {
            get => _shape;
        }

        public HashSet<int> InsertedNodesID
        {
            get => _insertedNodes;
        }

        public HashSet<int> ContactIDs
        {
            get => _contactIDs;
        }

        public RectBound RectBound
        {
            get => _shape.RectBound;
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

        public NyanCollider(INyanCollisionProvider provider, Shape shape, int id, string comment = null)
        {
            InstanceID = id;
            _shape = shape;
            _provider = provider;
            _insertedNodes = new(4);
            _contactIDs = new();
            _comment = comment;
        }

        public void InitLayer(NyanLayer layer, NyanLayerMask mask)
        {
            _layer = layer;
            _mask = mask;
            _isLayerInitialized = true;
        }

        public void SetInstigatorID(int id)
        {
            _instigatorID = id;
        }

        public void SetActive(bool value)
        {
#if UNITY_EDITOR
            if (value && _shape == null)
                throw new Exception("Shape is null.");
            if (_shape == Shape.Invalid)
                throw new Exception("Shape is not initialized.");
#else
            if (_shape == null)
                return;
#endif
            _isActive = value;
            if (_isActive == true)
                _shape.UpdatePosition(_provider.transform);
            EnabledChanged?.Invoke(this);
        }

        public void SetShape(Shape shape)
        {
            _shape = shape;
        }

        public void RefreshTransform()
        {
            _shape.UpdatePosition(_provider.transform);
            PositionChanged?.Invoke(this);
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

            if (_shape == null)
                sb.AppendLine("Shape is null");

            if (!_isLayerInitialized)
                sb.AppendLine("Layer is not initialized");

            log = sb.ToString();
            return sb.Length == 0;
        }
#endif
    }
}
