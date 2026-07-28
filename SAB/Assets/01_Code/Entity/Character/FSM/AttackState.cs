using Chu.AI;
using UnityEngine;

namespace SAB.Unit
{
    public class AttackState : IMachineState<CharacterState, StateContext>
    {
        public CharacterState Type
        {
            get => CharacterState.Attack;
        }

        public void Enter(StateContext context)
        {
            Debug.Log(context.IsAttacking);
        }

        public bool Tick(StateContext context)
        {
            if (context.IsAttacking == false)
                return false;

            return true;
        }

        public void Exit(StateContext context)
        {
        }
    }

}
