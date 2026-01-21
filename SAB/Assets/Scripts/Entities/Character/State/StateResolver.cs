using Chu.AI;

namespace SAB.Unit.State
{
    public class StateResolver : IMachineStateResolver<CharacterState, StateContext>
    {
        public CharacterState Resolve(StateContext stats)
        {
            if (stats.IsAttacking == true)
                return CharacterState.Attack;
            if (stats.IsMoveing == true)
                return CharacterState.Move;

            return CharacterState.Idle;
        }
    }
}
