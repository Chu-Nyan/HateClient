using Chu;
using Chu.Collision;
using UnityEngine;

namespace SAB.Unit.Combat
{
    /// <summary>
    /// 스킬의 이펙트, 환경 오브젝트 충돌 처리
    /// </summary>
    public class SkillObject : MonoBehaviour, INyanCollisionProvider, IAttackContextProvider
    {
        private const NyanLayer _layer = NyanLayer.Projectile;
        private static int _hitUnityLayer;

        private NyanCollider _nyanCollider;
        private AttackContext _context;
        private Vector3 _dir;
        private int _logicStep;
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
            _nyanCollider = ChuEngine.Instance.GeneratorHub.NyanColliderGenerator
                .GenerateCollider(this, $"스킬 투사체")
                .GetCollider(true);
        }

        private void Update()
        {
            var logics = Context.SkillData.CollisionLogics;
            if (_timer > logics[_logicStep].ActiveTime)
            {
                _logicStep++;
                if (_logicStep < logics.Length)
                    ChangeShape(_logicStep);
                else
                {
                    gameObject.SetActive(false);
                    return;
                }
            }

            _timer += Time.deltaTime;
            transform.position += logics[_logicStep].Speed * Time.deltaTime * transform.forward;
            _nyanCollider.RefreshTransform();
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

        public void Setup(int instigator, AttackContext context)
        {
            _context = context;
            _logicStep = 0;
            _nyanCollider.SetInstigatorID(instigator);
            _nyanCollider.SetLayer(_layer, _context.Mask);

            ChangeShape(_logicStep);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
            _nyanCollider.SetActive(value);
        }

        private void ChangeShape(int step)
        {
            var logic = Context.SkillData.CollisionLogics[step];
            var shape = ShapeFactory.GenerateShape(logic.Hitboxes);
            _nyanCollider.SetShape(shape);
            _timer = 0;
        }

        public void SetTarget(Vector3 start, Vector3 dir)
        {
            transform.position = start;
            _dir = dir;
            _timer = 0f;
            transform.rotation = Quaternion.LookRotation(_dir);
        }

        public void OnNyanCollisionEnter(INyanCollisionProvider provider)
        {
            // 충돌 처리
            if (provider.Collider.Layer == NyanLayer.Unit)
            {
                var acter = provider as IDefendable;
                acter.Defend(_context, new HitResult(_logicStep));

            }
            Debug.Log(provider.Collider.Comment + " 충돌");
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
