using Chu;
using Chu.Collision;
using Chu.Collision.Layer;
using UnityEngine;

namespace SAB.Unit.Combat
{
    /// <summary>
    /// 스킬의 이펙트, 환경 오브젝트 충돌 처리
    /// </summary>
    public class SkillProjectile : MonoBehaviour, INyanCollisionProvider, IAttackContextProvider
    {
        private const NyanLayer _layer = NyanLayer.Projectile;
        private const float _maxLifeSpan = 5f;

        private static int _hitUnityLayer;

        private NyanCollider _nyanCollider;
        private AttackContext _context;
        private Vector3 _dir;
        private float _timer;

        public NyanCollider Collider
        {
            get => _nyanCollider;
        }

        public AttackContext Context
        {
            get => _context;
        }

        public void Awake()
        {
            var mask = new NyanLayerMask(NyanLayer.Unit);

            _nyanCollider = ChuEngine.Instance.GeneratorHub.NyanColliderGenerator
                .GenerateCollider(this, Shape.Invalid, $"투사체")
                .SetLayer(_layer, mask)
                .GetCollider();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _maxLifeSpan)
                gameObject.SetActive(false);
            else
            {
                transform.position += 10f * Time.deltaTime * transform.forward;
                _nyanCollider.RefreshTransform();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((1 << other.gameObject.layer & _hitUnityLayer) == 0)
                return;

            Debug.Log("환경 오브젝트 충돌");
            OnExpired();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StaticInit()
        {
            _hitUnityLayer = LayerMask.GetMask("Ground") + LayerMask.GetMask("Obstacle");
        }

        public void Refresh(Shape shape, int instigator, AttackContext context)
        {
            _nyanCollider.SetShape(shape);
            _nyanCollider.SetInstigatorID(instigator);
            _context = context;
        }

        public void ActivteNyanCollision(bool value)
        {
            _nyanCollider.SetActive(value);
        }

        public void SetColiderShape(Shape shape)
        {
            _nyanCollider.SetShape(shape);
        }

        public void SetTarget(Vector3 start, Vector3 dir)
        {
            transform.position = start;
            _dir = dir;
            _timer = 0f;
            transform.rotation = Quaternion.LookRotation(_dir);
        }

        public void OnNyanCollisionEnter(INyanCollisionProvider collider)
        {
            // 충돌 처리
            Debug.Log(collider.Collider.Comment + " 충돌");
        }

        public void OnNyanCollisionExit(INyanCollisionProvider collider)
        {
            // 없음
        }

        private void OnExpired()
        {
            _nyanCollider.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
