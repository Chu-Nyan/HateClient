using SAB.GameSystem;
using UnityEngine;

namespace SAB.EntityAgent
{
    /// <summary>
    /// 플레이어의 입력으로 Agent 행동을 제어함
    /// </summary>
    public class PlayerBrain : IBrainStrategy
    {
        private IMovementReceiver _movementReceiver;
        private IOffenseReceiver _combatReceiver;
        private CombatModeDecider _combatModeDecider;
        private Vector2 _direction;
        private bool _isMoving;

        public BrainType BrainType
        {
            get => BrainType.Player;
        }

        public PlayerBrain()
        {
            _combatModeDecider = new();
        }

        public void Tick()
        {
            _combatModeDecider.TickForExit(Time.deltaTime);
            if (_combatModeDecider.IsActivate == false)
            {
                _combatReceiver.SetCombatMode(false);
            }

            if (_isMoving == true)
                MoveDirection();
        }

        public void SetMovementReceiver(IMovementReceiver receiver)
        {
            _movementReceiver = receiver;
        }

        public void SetCombatReceiver(IOffenseReceiver receiver)
        {
            _combatReceiver = receiver;
            _combatModeDecider.Setup(receiver.InstanceID);

            // TODO : 캐릭터로 컴백 모드 옮기기
        }

        public void Enable()
        {
            InputManager.Instance.WASD.RegisterPerformed(SetDiection);
            InputManager.Instance.WASD.RegisterCanceled(SetDiection);
            InputManager.Instance.LeftClick.RegisterPerformed(UseBasicAttack);
            for (int i = 0; i < 3; i++) // 3 = 플레이어 스킬 숏컷 갯수
            {
                InputManager.Instance.Skills[i].RegisterPerformed(UseSkillToScreenPoint);
            }
        }

        public void Disable()
        {
            InputManager.Instance.WASD.UnregisterPerformed(SetDiection);
            InputManager.Instance.WASD.UnregisterCanceled(SetDiection);
            InputManager.Instance.LeftClick.UnregisterPerformed(UseBasicAttack);
            SetDiection(Vector2.zero);
        }

        private void SetDiection(Vector2 dir)
        {
            _direction = dir;
            _isMoving = _direction != Vector2.zero;
        }

        private void MoveDirection()
        {
            var direction = new Vector3(_direction.x * 0.5f, 0, _direction.y * 0.5f) + _movementReceiver.transform.position;
            _movementReceiver.SetDestination(direction);
        }

        private void UseBasicAttack()
        {
            UseSkillToScreenPoint(0);
        }

        private void UseSkillToScreenPoint(int skillIndex)
        {
            var mousePoint = InputManager.Instance.MousePosition;
            _combatModeDecider.SetActivate(true);
            _combatReceiver.SetCombatMode(true);
            Ray ray = Camera.main.ScreenPointToRay(mousePoint);
            int flag = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, flag))
            {
                Vector3 targetPoint = hit.point;
                _combatReceiver.Attack(skillIndex, targetPoint);
            }
        }
    }
}
