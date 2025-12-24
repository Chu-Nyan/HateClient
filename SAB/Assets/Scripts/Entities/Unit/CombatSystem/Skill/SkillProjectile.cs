using Chu;
using Chu.Collision;
using UnityEngine;

namespace SAB.Unit.Combat
{
    /// <summary>
    /// 스킬의 이펙트, 환경 오브젝트 충돌 처리
    /// </summary>
    public class SkillProjectile : MonoBehaviour, INyanCollisionProvider
    {
        private const float _maxLifeSpan = 5f;
        private static int _hitLayer;

        private NyanCollider _nyanCollider;
        private Vector3 _dir;
        private float _timer;

        public NyanCollider Collider
        {
            get => _nyanCollider;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StaticInit()
        {
            _hitLayer = LayerMask.GetMask("Ground") + LayerMask.GetMask("Obstacle");
        }

        private void Awake()
        {
            _nyanCollider = ChuEngine.Instance.GeneratorHub.NyanColliderGenerator
                .GenerateCollider(this)
                .GetCollider();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _maxLifeSpan)
                gameObject.SetActive(false);
            else
                transform.position += 10f * Time.deltaTime * transform.forward;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((1 << other.gameObject.layer & _hitLayer) == 0)
                return;

            Debug.Log("환경 오브젝트 충돌");
            OnExpired();
        }

        public void OnNyanCollisionEnter(INyanCollisionProvider collider)
        {
            // 충돌 처리
            Debug.Log("Nyan Collider 충돌");
            OnExpired();
        }

        public void OnNyanCollisionExit(INyanCollisionProvider collider)
        {
            // 없음
        }

        public void SetColiderShape(Shape shape)
        {
            _nyanCollider.SetShape(shape, "투사체");
        }

        public void SetTarget(Vector3 start, Vector3 dir)
        {
            transform.position = start;
            _dir = dir;
            _timer = 0f;
            transform.rotation = Quaternion.LookRotation(_dir);
        }

        private void OnExpired()
        {
            gameObject.SetActive(false);
        }
    }
}
