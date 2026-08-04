using Chu.Collision;
using Chu.Utility;
using SAB.GameSystem;
using System;
using UnityEngine;

namespace SAB.Skill
{
    /// <summary>
    /// 스킬의 이펙트, 환경 오브젝트 충돌 처리
    /// </summary>
    public class SkillObject : MonoBehaviour, INyanCollisionProvider, IAttackContextProvider
    {
        private const NyanLayer Layer = NyanLayer.Projectile;
        private static readonly int _hitUnityLayer = LayerMask.GetMask("Ground") | LayerMask.GetMask("Obstacle");

        private NyanCollider _nyanCollider;
        private AttackContext _context;
        private int _hostilityMask;
        private int _logicStep;
        private float _timer;

        public event Action<SkillObject> Destroyed;

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
            _nyanCollider = GameColliderFactory.CreateSkillProjectile(this, 0);
        }

        private void Update()
        {
            var logics = Context.SkillData.CollisionLogics;
            _timer += Time.deltaTime;

            if (_timer > logics[_logicStep].ActiveTime)
            {
                _logicStep++;
                if (_logicStep < logics.Length)
                    ChangeShape(_logicStep);
                else
                {
                    SetActive(false);
                    return;
                }
            }

            transform.position += logics[_logicStep].Speed * Time.deltaTime * transform.forward;
            _nyanCollider.SetTransform(transform.position.ToVector2XZ(), transform.eulerAngles.y);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((1 << other.gameObject.layer & _hitUnityLayer) == 0)
                return;

            Debug.Log("환경 오브젝트 충돌");
            SetActive(false);
        }

        public void Setup(int instigator, AttackContext context, int hostilityMask)
        {
            _context = context;
            _hostilityMask = hostilityMask;
            _nyanCollider.SetInstigatorID(instigator);
            _nyanCollider.SetLayer(Layer, _context.Mask);
            _logicStep = 0;
            ChangeShape(_logicStep);
            _nyanCollider.SetTransform(transform.position.ToVector2XZ(), transform.eulerAngles.y);
            _nyanCollider.SetActive(true);
        }

        public void SetActive(bool value)
        {
            if (value == false)
            {
                Destroyed?.Invoke(this);
                Destroyed = null;
            }

            _nyanCollider.SetActive(value);
            gameObject.SetActive(value);
        }

        private void ChangeShape(int step)
        {
            var logic = Context.SkillData.CollisionLogics[step];
            var shape = ShapeParam.ConvertShape(logic.Hitboxes);
            _nyanCollider.SetShape(shape);
            _timer = 0;
        }

        public void SetTarget(Vector3 start, Vector3 dir)
        {
            transform.position = start;
            _timer = 0f;
            transform.rotation = Quaternion.LookRotation(dir);
            _nyanCollider.SetTransform(transform.position.ToVector2XZ(), transform.eulerAngles.y);
        }

        public void OnNyanCollisionEnter(INyanCollisionProvider provider)
        {
            if (((int)provider.Collider.Layer & Const.Layer_Unit) != 0)
            {
                if (provider is IDefendable acter == true && (_hostilityMask & (1 << (int)acter.FactionType)) != 0)
                {
                    acter.Defend(_context, new HitResult(_logicStep));
                }
            }
        }

        public void OnNyanCollisionExit(INyanCollisionProvider collider)
        {
        }
    }
}
