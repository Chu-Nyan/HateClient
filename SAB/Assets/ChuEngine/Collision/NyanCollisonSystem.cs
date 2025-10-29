using System.Collections.Generic;
using Chu.Data;
using UnityEngine;

namespace Chu.Collision
{
    /// <summary>
    /// 물리 객체를 관리, 충돌 시뮬레이션 실행
    /// </summary>
    /// - 개발 방향
    /// 시스템에서 등록된 객체는 해제 불가능
    public class NyanCollisonSystem : MonoBehaviour
    {
        private QuadTree<NyanCollider> _root;

        private HashSet<int> _frameChecked;
        private HashSet<int> _candidateChecked;
        private Queue<CollisionInfo> _collisionInfoQueue;
        private Dictionary<int, INyanCollisionProvider> _collidersByID;

        /// <summary>
        /// 배열의 초기 크기 지정
        /// </summary>
        public void InitArray(int capacity)
        {
            _candidateChecked = new(capacity);
            _collisionInfoQueue = new(capacity);
            _frameChecked = new(capacity);
            _collidersByID = new(capacity);
        }

        /// <summary>
        /// 충돌 처리 구역 설정
        /// </summary>
        public void InitBound(RectBound bound)
        {
            _root = new(bound);
        }

        private void LateUpdate()
        {
            // 충돌 실행
            _frameChecked.Clear();
            if (_collisionInfoQueue.Count == 0)
                return;

            foreach (var info in _collisionInfoQueue)
            {
                var aColider = _collidersByID[info.PrimaryID];
                var bColider = _collidersByID[info.TargetID];

                if (info.State == CollisionState.Enter)
                {
                    aColider.Collider.AddContactColliderID(info.TargetID);
                    bColider.Collider.AddContactColliderID(info.PrimaryID);
                    aColider.OnNyanCollisionEnter(bColider);
                    bColider.OnNyanCollisionEnter(aColider);
                }
                else // EXIT
                {
                    aColider.OnNyanCollisionExit(bColider);
                    bColider.OnNyanCollisionExit(aColider);
                    aColider.Collider.RemoveContactColliderID(info.TargetID);
                    bColider.Collider.RemoveContactColliderID(info.PrimaryID);
                }
            }

            _collisionInfoQueue.Clear();
        }

        /// <summary>
        /// 물리 시스템에 객체 등록, 해제 불가
        /// </summary>
        public void RegisterEntity(INyanCollisionProvider obj)
        {
            if (_collidersByID.TryAdd(obj.Collider.ID, obj) == false)
                throw new System.Exception("콜라이더 중복 등록");

            obj.Collider.RegisterEnabled(OnShapeActivationChanged);
            OnShapeActivationChanged(obj.Collider);
        }

        private void OnShapeActivationChanged(NyanCollider collider)
        {
            if (collider.IsEnable == true)
            {
                collider.RegisterPositionChanged(Insert);
                Insert(collider);
            }
            else // false
            {
                collider.UnregisterPositionChanged(Insert);
                Remove(collider);
            }
        }

        private void Insert(NyanCollider obj)
        {
            var nodes = obj.InsertedNodesID;
            if (nodes.Count == 1)
            {
                foreach (var item in nodes)
                {
                    if (_root.GetNode(item).IsFullyInside(obj.RectBound) == true)
                    {
                        CheckCollision(obj);
                        return;
                    }
                }
            }

            _root.Insert(obj);
            CheckCollision(obj);
        }

        private void Remove(NyanCollider obj)
        {
            _root.Remove(obj);
        }

        private void CheckCollision(NyanCollider primary)
        {
            if (_frameChecked.Add(primary.ID) == false)
                return;

            _candidateChecked.Clear();

            foreach (var nodeIndex in primary.InsertedNodesID)
            {
                var node = _root.GetNode(nodeIndex);

                foreach (var candidate in node.Entities)
                {
                    CheckCollisionState(primary, candidate);
                }
            }

            foreach (var num in primary.ContactIDs)
            {
                CheckCollisionState(primary, _collidersByID[num].Collider);
            }
        }

        private void CheckCollisionState(NyanCollider primary, NyanCollider candidate)
        {
            if (_frameChecked.Contains(candidate.ID) == true)
                return;
            if (_candidateChecked.Add(candidate.ID) == false)
                return;

            bool isCollision = primary.Shape.Intersects(candidate.Shape);
            bool isContacted = primary.IsContacted(candidate.ID);


            if (isCollision == true && isContacted == false)
                _collisionInfoQueue.Enqueue(new CollisionInfo(primary.ID, candidate.ID, CollisionState.Enter));
            else if (isCollision == false && isContacted == true)
                _collisionInfoQueue.Enqueue(new CollisionInfo(primary.ID, candidate.ID, CollisionState.Exit));
        }
    }
}
