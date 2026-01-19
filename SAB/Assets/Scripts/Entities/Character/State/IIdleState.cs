using Chu.AI;

namespace SAB.Unit.State
{
    public class IIdleState : IMachineState<CharacterState, CharacterStats>
    {
        public CharacterState Type
        {
            get => CharacterState.Idle;
        }

        public void Enter(CharacterStats context)
        {
        }

        public bool Tick(CharacterStats context)
        {
            return true;
        }

        public void Exit(CharacterStats context)
        {
        }
    }
}
