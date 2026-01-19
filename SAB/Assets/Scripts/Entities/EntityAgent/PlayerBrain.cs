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
        private Vector2 _direction;
        private bool _isMoving;

        public void Tick()
        {
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
        }

        public void Enable()
        {
            InputManager.Instance.RegisterWASDPerformed(SetDiection);
            InputManager.Instance.RegisterWASDCanceled(SetDiection);
            InputManager.Instance.RegisterLeftClickedPerformed(BasicAttack);
        }

        public void Disable()
        {
            InputManager.Instance.UnregisterWASDPerformed(SetDiection);
            InputManager.Instance.UnregisterWASDCanceled(SetDiection);
            InputManager.Instance.UnregisterLeftClickedPerformed(BasicAttack);
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

        private void BasicAttack(Vector2 screenPoint)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            int flag = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, flag))
            {
                Vector3 targetPoint = hit.point;
                _combatReceiver.Attack(0, targetPoint);
            }
        }
    }

}