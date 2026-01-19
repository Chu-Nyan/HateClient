using Chu.AI;

namespace SAB.Unit.State
{
    public class StateResolver : IMachineStateResolver<CharacterState, CharacterStats>
    {
        public CharacterState Resolve(CharacterStats stats)
        {
            CharacterCurrentStats current = stats.CurrentStats;
            if (current.IsAttacking == true)
                return CharacterState.Attack;
            if (current.IsMoveing == true)
                return CharacterState.Move;

            return CharacterState.Idle;
        }
    }
}
