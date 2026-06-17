using Chu.Collision;
using Chu.Core;
using UnityEngine;

namespace SAB.EntityAgent
{
    public class CombatModeDecider : INyanCollisionProvider
    {
        private const float _searchRange = 5;
        private const float _waitTime = 5;

        private NyanCollider _collider;

        private int _enemiesInRange;
        private float _offTimer;
        private bool _isActivate;

        public NyanCollider Collider
        {
            get => _collider;
        }

        public bool IsActivate
        {
            get => _isActivate;
        }

        public void Setup(int instigatorID)
        {
            if (_collider == null)
            {
                var shape = ShapeFactory.Instance.Generate<CircleShape>();
                shape.Setup(new CircleRangeData(Vector2.zero, 1));
                var mask = new NyanLayerMask(NyanLayer.PlayerUnit, NyanLayer.NPCUnit);
                _collider = ChuEngine.Instance.GeneratorHub.NyanColliderGenerator
                    .GenerateCollider(this, "전투 판정")
                    .SetShape(shape)
                    .SetLayer(NyanLayer.UnitSensor, mask)
                    .SetInstigatorID(instigatorID)
                    .GetCollider(true);

                _collider.SetActive(false);
            }
        }

        public void TickForExit(float time)
        {
            //_collider.RefreshTransform(); // TODO : 캐릭터에 이벤트 추가하기
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
            if (((int)provider.Collider.Layer | Const.Layer_Unit) != 0)
                _enemiesInRange++;
        }

        public void OnNyanCollisionExit(INyanCollisionProvider provider)
        {
            if (((int)provider.Collider.Layer | Const.Layer_Unit) != 0)
                _enemiesInRange--;
        }
    }
}
