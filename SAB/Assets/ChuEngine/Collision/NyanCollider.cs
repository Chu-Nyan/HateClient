using System;
using System.Collections.Generic;
using Chu.Data;
using UnityEngine;

namespace Chu.Collision
{
    public class NyanCollider : IQuadTreeEntity
    {
        public readonly int ID;
        private readonly string _comment;
        private readonly Shape _shape;
        private readonly Transform _transform;
        private readonly HashSet<int> _insertedNodes;
        private readonly HashSet<int> _contactIDs;

        private event Action<NyanCollider> PositionChanged;

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

        public NyanCollider(Transform pivot, Shape entity, int id, string comment)
        {
            _transform = pivot;
            _shape = entity;
            ID = id;
            _comment = comment;
            _insertedNodes = new(4);
            _contactIDs = new();
        }

        public void OnPositionChanged()
        {
            _shape.UpdateRectBound(_transform);
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

        public void RegisterContactColliderID(int id)
        {
            _contactIDs.Add(id);
        }

        public void UnregisterContactColliderID(int id)
        {
            _contactIDs.Remove(id);
        }

        public void RegisterPositionChanged(Action<NyanCollider> action)
        {
            PositionChanged += action;
        }

        public override string ToString()
        {
            return $"{ID} {_comment}";
        }
    }
}
