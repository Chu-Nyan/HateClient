using System;
using System.Collections.Generic;
using Chu.Data;
using Chu.Utility;
using UnityEngine;

namespace Chu.Collision
{
    public class NyanCollider : IQuadTreeEntity
    {
        private static readonly IDNumbering _iDNumbering = new(0, 64);

        public readonly int ID;
        public readonly string Name;
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

        public NyanCollider(Transform pivot, Shape entity, string name)
        {
            ID = _iDNumbering.GetID();
            _transform = pivot;
            _shape = entity;
            _insertedNodes = new(4);
            _contactIDs = new();
            Name = name;
        }

        public NyanCollider(Transform pivot, Shape entity) : this(pivot, entity, "Collider") { }

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
    }
}
