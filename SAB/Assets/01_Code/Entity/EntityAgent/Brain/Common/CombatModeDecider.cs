using Chu.Collision;
using SAB.GameSystem;
using UnityEngine;

namespace SAB.EntityAgent
{
    public class CombatModeDecider : INyanCollisionProvider
    {
        private const float SearchRange = 5;
        private const float WaitTime = 5;

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
                var shape = new CircleShape();
                shape.Setup(new CircleRangeData(Vector2.zero, 1));
                var mask = new NyanLayerMask(NyanLayer.PlayerUnit, NyanLayer.NPCUnit);
                _collider = GameColliderFactory.CreateCombatModeArea(this, instigatorID);
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
                if (_offTimer >= WaitTime)
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
