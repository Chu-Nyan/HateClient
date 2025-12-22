using Chu.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chu.Collision
{
    /// <summary>
    /// NyanCollisonSystem에서 사용하는 물리 오브젝트
    /// </summary>
    public class NyanCollider : IQuadTreeEntity
    {
        public readonly int ID;
        private readonly Transform _transform;
        private readonly HashSet<int> _insertedNodes;
        private readonly HashSet<int> _contactIDs;
        private Shape _shape;
        private bool _isActive;
        private string _comment;
        private INyanCollisionProvider _provider;

        private event Action<NyanCollider> PositionChanged;
        private event Action<NyanCollider> EnabledChanged;

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

        public INyanCollisionProvider Provider
        {
            get => _provider;
        }

        public bool IsActive
        {
            get => _isActive;
        }

        public string Comment
        {
            get => _comment;
        }

        public NyanCollider(INyanCollisionProvider provider, int id)
        {
            ID = id;
            _provider = provider;
            _transform = provider.transform;
            _insertedNodes = new(4);
            _contactIDs = new();
        }

        public void SetActive(bool value)
        {
            _isActive = value;
            EnabledChanged?.Invoke(this);
        }

        public void SetShape(Shape entity, string comment)
        {
            _shape = entity;
            _comment = comment;
        }

        public void OnPositionChanged()
        {
            _shape.UpdatePosition(_transform);
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
            return $"{ID} {_comment}";
        }
    }
}
