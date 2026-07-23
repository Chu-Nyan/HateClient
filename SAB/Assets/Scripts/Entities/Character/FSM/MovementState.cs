using Chu.AI;

namespace SAB.Unit
{
    public class MovementState : IMachineState<CharacterState, StateContext>
    {
        public CharacterState Type
        {
            get => CharacterState.Movement;
        }

        public void Enter(StateContext context)
        {
        }

        public bool Tick(StateContext context)
        {
            if (context.IsAttacking == true)
                return false;

            return true;
        }

        public void Exit(StateContext context)
        {
        }
    }
}
