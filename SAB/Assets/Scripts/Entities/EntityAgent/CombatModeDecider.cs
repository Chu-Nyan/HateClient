using Chu;
using Chu.Collision;
using Chu.Collision.Layer;
using UnityEngine;

namespace SAB.EntityAgent
{
    public class CombatModeDecider : INyanCollisionProvider
    {
        private const float _searchRange = 5;
        private const float _waitTime = 5;

        private Transform _transform;
        private NyanCollider _collider;

        private int _enemiesInRange;
        private float _offTimer;
        private bool _isActivate;

        public Transform transform
        {
            get => _transform;
        }

        public NyanCollider Collider
        {
            get => _collider;
        }

        public void Setup(Transform origin, int instigatorID)
        {
            _transform = origin;

            if (_collider == null)
            {
                var mask = new NyanLayerMask(NyanLayer.Unit);
                _collider = ChuEngine.Instance.GeneratorHub.NyanColliderGenerator
                    .GenerateCollider(this, new CircleShape(_searchRange), "전투 판정")
                    .SetLayer(NyanLayer.UnitSensor, mask)
                    .SetInstigatorID(instigatorID)
                    .GetCollider();

                _collider.SetActive(false);
            }
        }

        public void TickForExit(float time)
        {
            if (_isActivate == false)
                return;

            _collider.RefreshTransform(); // TODO : 캐릭터에 이벤트 추가하기
            if (_enemiesInRange > 0)
            {
                _offTimer = 0;
            }
            else
            {
                _offTimer += time;
                if (_offTimer >= _waitTime)
                    SetActivate(false);
            }
        }

        public void SetActivate(bool value)
        {
            _isActivate = value;
            if (value == true)
            {
                _collider.SetActive(true);
                _offTimer = 0f;
            }
        }

        public void OnNyanCollisionEnter(INyanCollisionProvider provider)
        {
            // 적일 경우만 처리
            if (provider.Collider.Layer == NyanLayer.Unit)
                _enemiesInRange++;
        }

        public void OnNyanCollisionExit(INyanCollisionProvider provider)
        {
            if (provider.Collider.Layer == NyanLayer.Unit)
                _enemiesInRange--;
        }
    }
}
